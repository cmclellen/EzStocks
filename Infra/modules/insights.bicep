param location string = resourceGroup().location

param resourceNameFormat string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: format(resourceNameFormat, 'log')
  location: location
}

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: format(resourceNameFormat, 'appi')
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}
