param location string = resourceGroup().location

param resourceNameFormat string

resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: format(resourceNameFormat, 'appi')
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
  }
}
