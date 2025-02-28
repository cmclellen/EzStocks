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
  kind: 'linux'
  properties: {
    reserved: true
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
      linuxFxVersion: 'DOTNET-ISOLATED|9.0'
      appSettings: [
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'
          //value: storageAccount.properties.primaryEndpoints.blob
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
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: 'dotnet-isolated'
        }
        {
          name: 'WEBSITE_USE_PLACEHOLDER_DOTNETISOLATED'
          value: '1'
        }
        {
          name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
          value: appInsights.properties.ConnectionString
        }
        {
          name: 'ServicebusConnection__fullyQualifiedNamespace'
          value: serviceBusNamespace.properties.serviceBusEndpoint
        }
      ]
      minTlsVersion: '1.2'
      ftpsState: 'FtpsOnly'
    }
  }
}

resource sbDataReceiverRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: '4f6d3b9b-027b-4f4c-9142-0e5a2a2247e0'
}

resource sbDataReceiverRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(serviceBusNamespace.id, sbDataReceiverRoleDefinition.id)
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
  name: guid(serviceBusNamespace.id, sbDataSenderRoleDefinition.id)
  scope: serviceBusNamespace
  properties: {
    roleDefinitionId: sbDataSenderRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

resource stgBlobDataContributorRoleDefinition 'Microsoft.Authorization/roleDefinitions@2022-05-01-preview' existing = {
  scope: subscription()
  name: 'ba92f5b4-2d11-453d-a403-e96b0029c9fe'
}

resource stgBlobDataContributorRoleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(storageAccount.id, stgBlobDataContributorRoleDefinition.id)
  scope: storageAccount
  properties: {
    roleDefinitionId: stgBlobDataContributorRoleDefinition.id
    principalId: functionApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
