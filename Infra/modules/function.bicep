param location string = resourceGroup().location

param resourceNameFormat string

@secure()
param polygonioApiKey string

param entraB2cTenantId string
param entraB2cClientId string

resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: format(resourceNameFormat, 'appi')
}

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' existing = {
  name: format(resourceNameFormat, 'sbns')
}

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: format(resourceNameFormat, 'kv')
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-05-01' existing = {
  name: replace(format(resourceNameFormat, 'stg'), '-', '')
}

resource appServicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  name: format(resourceNameFormat, 'asp')
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

var kvName = format(resourceNameFormat, 'kv')

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
      linuxFxVersion: 'DOTNET-ISOLATED|9.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage__blobServiceUri'
          value: 'https://${storageAccount.name}.blob.${environment().suffixes.storage}'
        }
        // {
        //   name: 'AzureWebJobsStorage'
        //   value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
        //   //value: storageAccount.properties.primaryEndpoints.blob
        // }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
        // APPLICATIONINSIGHTS_CONNECTION_STRING not considered secrets https://learn.microsoft.com/en-us/azure/azure-monitor/app/connection-strings
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ServicebusConnection__fullyQualifiedNamespace'
          value: serviceBusNamespace.properties.serviceBusEndpoint
        }
        {
          name: 'PolygonIO__ApiBaseUrl'
          value: 'https://api.polygon.io/'
        }
        {
          name: 'PolygonIO__ApiKey'
          value: '@Microsoft.KeyVault(VaultName=${kvName};SecretName=polygonio-api-key)'
        }
        {
          name: 'AzureFunctionsJobHost__logging__logLevel__default'
          value: 'Debug'
        }
        {
          name: 'EntraB2C__TenantId'
          value: entraB2cTenantId
        }
        {
          name: 'EntraB2C__ClientId'
          value: entraB2cClientId
        }
        // {
        //   name: 'AzureWebJobsFeatureFlags'
        //   value: 'EnableWorkerIndexing'
        // }
        // {
        //   name: 'WEBSITE_RUN_FROM_PACKAGE'
        //   value: '1'
        // }
      ]
      connectionStrings: [
        {
          name: 'DefaultConnection'
          connectionString: '@Microsoft.KeyVault(VaultName=${kvName};SecretName=cosmosdb-connection-string)'
          type: 'SQLServer'
        }
      ]
      minTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
    }
  }
}

resource polygonioApiKeySecret 'Microsoft.KeyVault/vaults/secrets@2024-04-01-preview' = {
  name: 'polygonio-api-key'
  parent: keyVault
  properties: {
    value: polygonioApiKey
  }
}

// Service Bus
resource sbDataReceiverRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '4f6d3b9b-027b-4f4c-9142-0e5a2a2247e0'
}

resource sbDataReceiverRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(serviceBusNamespace.id, sbDataReceiverRoleDefinition.id, functionApp.id)
  scope: serviceBusNamespace
  properties: {
    roleDefinitionId: sbDataReceiverRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource sbDataSenderRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '69a216fc-b8fb-44d8-bc22-1f3c2cd27a39'
}

resource sbDataSenderRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(serviceBusNamespace.id, sbDataSenderRoleDefinition.id, functionApp.id)
  scope: serviceBusNamespace
  properties: {
    roleDefinitionId: sbDataSenderRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// Storage Account
resource stgBlobDataOwnerRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: 'b7e6dc6d-f1e8-4753-8033-0f276bb0955b'
}

resource stgBlobDataOwnerRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccount.id, stgBlobDataOwnerRoleDefinition.id, functionApp.id)
  scope: storageAccount
  properties: {
    roleDefinitionId: stgBlobDataOwnerRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource stAccountContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '17d1049b-9a84-46fb-8f53-869881c3d3ab'
}

resource stAccountContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccount.id, stAccountContributorRoleDefinition.id, functionApp.id)
  scope: storageAccount
  properties: {
    roleDefinitionId: stAccountContributorRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// Key Vault
resource kvSecretsUserRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '4633458b-17de-408a-b874-0445c86b69e6'
}

resource kvSecretsUserRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(keyVault.id, kvSecretsUserRoleDefinition.id, functionApp.id)
  scope: keyVault
  properties: {
    roleDefinitionId: kvSecretsUserRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

output fnAppFqdn string = functionApp.properties.defaultHostName
