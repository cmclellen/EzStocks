param location string = resourceGroup().location

param resourceNameFormat string

param subnetId string

param staticWebsiteHostName string

param fnAppFqdn string

param lawId string

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: format(resourceNameFormat, 'kv')
}

resource appGwManagedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: '${format(resourceNameFormat, 'id')}-001'
  location: location
}

resource kvSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '4633458b-17de-408a-b874-0445c86b69e6'
}

resource kvSecretsUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, kvSecretsUserRoleDefinition.id, appGwManagedIdentity.id)
  scope: keyVault
  properties: {
    roleDefinitionId: kvSecretsUserRoleDefinition.id
    principalId: appGwManagedIdentity.properties.principalId
    principalType: 'ServicePrincipal'
  }
}

resource publicIPAddress 'Microsoft.Network/publicIPAddresses@2024-05-01' = {
  name: format(resourceNameFormat, 'pip')
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    publicIPAddressVersion: 'IPv4'
    publicIPAllocationMethod: 'Static'
    idleTimeoutInMinutes: 4
    ipTags: []
  }
}

var appGwName = format(resourceNameFormat, 'appgw')
resource appGateway 'Microsoft.Network/applicationGateways@2024-05-01' = {
  name: appGwName
  location: location
  dependsOn: [
    kvSecretsUserRoleAssignment
  ]
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${appGwManagedIdentity.id}': {}
    }
  }
  properties: {
    enableHttp2: true
    sku: {
      name: 'Basic'
      tier: 'Basic'
      family: 'Generation_1'
      capacity: 1
    }
    gatewayIPConfigurations: [
      {
        name: 'appGwIpConfig'
        properties: {
          subnet: {
            id: subnetId
          }
        }
      }
    ]
    sslCertificates: [
      {
        name: 'server'
        properties: {
          keyVaultSecretId: 'https://kv-ezstocks-dev-aue${environment().suffixes.keyvaultDns}/secrets/mycer'
        }
      }
    ]
    frontendIPConfigurations: [
      {
        name: 'appGwFrontendIpConfig'
        properties: {
          privateIPAllocationMethod: 'Dynamic'
          publicIPAddress: {
            id: publicIPAddress.id
          }
        }
      }
    ]
    frontendPorts: [
      {
        name: 'port_443'
        properties: {
          port: 443
          // port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'ui'
        properties: {
          backendAddresses: [
            {
              fqdn: staticWebsiteHostName
            }
          ]
        }
      }
      {
        name: 'api'
        properties: {
          backendAddresses: [
            {
              fqdn: fnAppFqdn
            }
          ]
        }
      }
      // {
      //   name: 'appGwBackendPool'
      //   properties: {
      //     backendAddresses: [
      //       for i in range(0, length(backendAddresses)): {
      //         fqdn: backendAddresses[i]
      //       }
      //     ]
      //   }
      // }
    ]
    backendHttpSettingsCollection: [
      {
        name: 'backendHttpSettings'
        properties: {
          port: 443
          protocol: 'Https'
          cookieBasedAffinity: 'Disabled'
          pickHostNameFromBackendAddress: true
          requestTimeout: 20
          // probeEnabledState: true
          // probeName: 'appgwprobe'
          // backendAddressPoolId: '${resourceId('Microsoft.Network/applicationGateways', name)}/backendAddressPools/appgwbackendpool'
        }
      }
    ]
    httpListeners: [
      {
        name: 'appGwListener'
        properties: {
          frontendIPConfiguration: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/frontendIPConfigurations/appGwFrontendIpConfig'
          }
          frontendPort: {
            id: resourceId('Microsoft.Network/applicationGateways/frontendPorts', appGwName, 'port_443')
          }
          // protocol: 'Http'
          protocol: 'Https'
          sslCertificate: {
            id: resourceId('Microsoft.Network/applicationGateways/sslCertificates', appGwName, 'server')
          }
          requireServerNameIndication: false
        }
      }
    ]
    requestRoutingRules: [
      {
        name: 'appGwUiRule'
        properties: {
          ruleType: 'Basic'
          priority: 100
          httpListener: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/httpListeners/appGwListener'
          }
          backendAddressPool: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/backendAddressPools/ui'
          }
          backendHttpSettings: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/backendHttpSettingsCollection/backendHttpSettings'
          }
        }
      }
    ]
  }
}

resource diagnosticSettings 'Microsoft.Insights/diagnosticSettings@2021-05-01-preview' = {
  scope: appGateway
  name: 'diagnosticSettings'
  properties: {
    workspaceId: lawId
    logs: [
      {
        category: 'ApplicationGatewayAccessLog'
        enabled: true
      }
      {
        category: 'ApplicationGatewayPerformanceLog'
        enabled: true
      }
    ]
    metrics: [
      {
        category: 'AllMetrics'
        enabled: true
      }
    ]
  }
}
