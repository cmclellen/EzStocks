param location string = resourceGroup().location

param resourceNameFormat string

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: format(resourceNameFormat, 'appi')
}

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' existing = {
  name: format(resourceNameFormat, 'sbns')
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: replace(format(resourceNameFormat, 'stg'), '-', '')
  location: location
  sku: {
    name: 'Standard_LRS'
  }
  kind: 'StorageV2'
  properties: {
    supportsHttpsTrafficOnly: true
  }
}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: format(resourceNameFormat, 'asp')
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2024-04-01' = {
  name: format(resourceNameFormat, 'func')
  location: location
  kind: 'functionapp,linux'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: storageAccount.properties.primaryEndpoints.blob
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'WEBSITE_RUN_FROM_PACKAGE'
          value: '1'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
      ]
    }
  }
}

// resource contributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
//   scope: subscription()
//   name: 'b24988ac-6180-42a0-ab88-20f7382dd24c'
// }

// storageRoleDefinitionId

// resource storageRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
//   name: guid(serviceBusNamespace.id, storageRoleDefinitionId)
//   scope: serviceBusNamespace
//   properties: {
//     roleDefinitionId: resourceId('Microsoft.Authorization/roleDefinitions', storageRoleDefinitionId)
//     principalId: flexFuncApp.identity.principalId
//     principalType: 'ServicePrincipal'
//   }
// }
