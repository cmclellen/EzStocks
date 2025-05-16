param location string = resourceGroup().location

param resourceNameFormat string

param subnetId string

param staticWebsiteHostName string

param fnAppFqdn string

resource publicIPAddress 'Microsoft.Network/publicIPAddresses@2024-05-01' = {
  name: format(resourceNameFormat, 'pip')
  location: location
  sku: {
    name: 'Standard'
    tier: 'Regional'
  }
  properties: {
    // ipAddress: '4.237.201.175'
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
        name: 'appGwFrontendPort'
        properties: {
          port: 80
        }
      }
    ]
    backendAddressPools: [
      {
        name: 'ui'
        // id: '${applicationGateways_myappgw_name_resource.id}/backendAddressPools/ui'
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
        // id: '${applicationGateways_myappgw_name_resource.id}/backendAddressPools/api'
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
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/frontendPorts/appGwFrontendPort'
          }
          protocol: 'Http'
          requireServerNameIndication: false
        }
      }
    ]
    requestRoutingRules: [
      {
        name: 'appGwUiRule'
        properties: {
          ruleType: 'Basic'
          priority: 1
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
      {
        name: 'appGwApiRule'
        properties: {
          ruleType: 'Basic'
          priority: 1
          httpListener: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/httpListeners/appGwListener'
          }
          backendAddressPool: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/backendAddressPools/api'
          }
          backendHttpSettings: {
            id: '${resourceId('Microsoft.Network/applicationGateways', appGwName)}/backendHttpSettingsCollection/backendHttpSettings'
          }
        }
      }
    ]
  }
}
