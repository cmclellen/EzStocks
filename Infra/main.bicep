@description('The Azure region into which the resources should be deployed.')
param location string = resourceGroup().location

@description('The environment.')
@allowed([
  'dev'
])
param environment string

var resourceNameFormat = format('{{0}}-ezstocks-{0}-aue', environment)

module db 'modules/database.bicep' = {
  name: 'database'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
  }
}

module queue 'modules/queue.bicep' = {
  name: 'queue'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
  }
}
