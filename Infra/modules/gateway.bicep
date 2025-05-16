param location string = resourceGroup().location

param resourceNameFormat string

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
  }
}

// resource appGateway 'Microsoft.Network/applicationGateways@2024-05-01' = {
//   name: format(resourceNameFormat, 'appgw')
//   location: location
//   properties: {
//     sku: {
//       name: 'Standard_v2'
//       tier: 'Standard_v2'
//       capacity: 1
//     }
//     gatewayIPConfigurations: [
//       {
//         name: 'appgwipconfig'
//         properties: {
//           subnet: {
//             id: subnetId
//           }
//         }
//       }
//     ]
//     frontendIPConfigurations: [
//       {
//         name: 'appgwfrontendip'
//         properties: {
//           publicIPAddress: {
//             id: publicIPAddress.id
//           }
//         }
//       }
//     ]
//     frontendPorts: [
//       {
//         name: 'appgwfrontendport'
//         properties: {
//           port: 80
//         }
//       }
//     ]
//     backendAddressPools: [
//       {
//         name: 'appgwbackendpool'
//         properties: {
//           backendAddresses: [
//             for i in range(0, length(backendAddresses)): {
//               fqdn: backendAddresses[i]
//             }
//           ]
//         }
//       }
//     ]
//     backendHttpSettingsCollection: [
//       {
//         name: 'appgwhttpsettings'
//         properties: {
//           port: 80
//           protocol: 'Http'
//           cookieBasedAffinity: 'Disabled'
//           requestTimeout: 20
//           probeEnabledState: true
//           probeName: 'appgwprobe'
//           backendAddressPoolId: '${resourceId('Microsoft.Network/applicationGateways', name)}/backendAddressPools/appgwbackendpool'
//         }
//       }
//     ]
//     httpListeners: [
//       {
//         name: 'appgwhl'
//         properties: {
//           frontendIPConfigurationId: '${resourceId('Microsoft.Network/applicationGateways', name)}/frontendIPConfigurations/appgwfrontendip'
//           frontendPortId: '${resourceId('Microsoft.Network/applicationGateways', name)}/frontendPorts/appgwfrontendport'
//           protocol: 'Http'
//         }
//       }
//     ]
//     requestRoutingRules: [
//       {
//         name: 'appgwrule'
//         properties: {
//           ruleType: 'Basic'
//           httpListenerId: '${resourceId('Microsoft.Network/applicationGateways', name)}/httpListeners/appgwhl'
//           backendAddressPoolId: '${resourceId('Microsoft.Network/applicationGateways', name)}/backendAddressPools/appgwbackendpool'
//           backendHttpSettingsId: '${resourceId('Microsoft.Network/applicationGateways', name)}/backendHttpSettingsCollection/appgwhttpsettings'
//         }
