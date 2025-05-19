param applicationGateways_myappgw_externalid string = '/subscriptions/7bc8f609-efb0-4078-8e94-461c8c3df7ea/resourceGroups/rg-ezstocks-dev-aue/providers/Microsoft.Network/applicationGateways/myappgw'
param location string = resourceGroup().location

param resourceNameFormat string

resource vnet 'Microsoft.Network/virtualNetworks@2024-05-01' = {
  name: format(resourceNameFormat, 'vnet')
  location: location
  properties: {
    addressSpace: {
      addressPrefixes: [
        '10.1.0.0/16'
      ]
    }
    privateEndpointVNetPolicies: 'Disabled'
    virtualNetworkPeerings: []
    enableDdosProtection: false    
  }

  resource snet_default 'subnets' = {
    name: 'default'
    properties: {
      serviceEndpoints: [
        {
          service: 'Microsoft.KeyVault'
        }
      ]
      addressPrefix: '10.1.0.0/24'
      applicationGatewayIPConfigurations: [
        {
          id: '${applicationGateways_myappgw_externalid}/gatewayIPConfigurations/appGatewayIpConfig'
        }
      ]
      delegations: []
      privateEndpointNetworkPolicies: 'Disabled'
      privateLinkServiceNetworkPolicies: 'Enabled'
    }
  }
}

output defaultSubnetId string = vnet::snet_default.id
