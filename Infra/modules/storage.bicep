param location string = resourceGroup().location

param resourceNameFormat string

param deploymentScriptTimestamp string = utcNow()

var indexDocument = 'index.html'
var errorDocument404Path = 'error.html'

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

resource managedIdentity 'Microsoft.ManagedIdentity/userAssignedIdentities@2018-11-30' = {
  name: 'DeploymentScript'
  location: location
}

var storageAccountContributorRoleDefinitionId = subscriptionResourceId(
  'Microsoft.Authorization/roleDefinitions',
  '17d1049b-9a84-46fb-8f53-869881c3d3ab'
)

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  scope: storageAccount
  name: guid(resourceGroup().id, storageAccountContributorRoleDefinitionId)
  properties: {
    roleDefinitionId: storageAccountContributorRoleDefinitionId
    principalId: managedIdentity.properties.principalId
  }
}

resource deploymentScript 'Microsoft.Resources/deploymentScripts@2023-08-01' = {
  name: 'deploymentScript'
  location: location
  kind: 'AzurePowerShell'
  identity: {
    type: 'UserAssigned'
    userAssignedIdentities: {
      '${managedIdentity.id}': {}
    }
  }
  dependsOn: [
    roleAssignment
  ]
  properties: {
    azPowerShellVersion: '3.0'
    scriptContent: loadTextContent('../scripts/enable-storage-static-website.ps1')
    forceUpdateTag: deploymentScriptTimestamp
    retentionInterval: 'PT4H'
    arguments: '-ResourceGroupName ${resourceGroup().name} -StorageAccountName ${storageAccount.name} -IndexDocument ${indexDocument} -ErrorDocument404Path ${errorDocument404Path}'
  }
}

output staticWebsiteHostName string = replace(
  replace(storageAccount.properties.primaryEndpoints.web, 'https://', ''),
  '/',
  ''
)
