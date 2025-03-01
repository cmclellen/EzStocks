param location string = resourceGroup().location

param resourceNameFormat string

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' = {
  name: format(resourceNameFormat, 'kv')
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: subscription().tenantId
    enableRbacAuthorization: true
  }
}
