@description('The Azure region into which the resources should be deployed.')
param location string = resourceGroup().location

@description('The environment.')
@allowed([
  'dev'
])
param environment string

@secure()
param alphavantageApiKey string

var resourceNameFormat = format('{{0}}-ezstocks-{0}-aue', environment)

module keyvault 'modules/keyvault.bicep' = {
  name: 'keyvault'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
  }
}

module insights 'modules/insights.bicep' = {
  name: 'insights'
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
  }
}

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

module function 'modules/function.bicep' = {
  name: 'function'
  dependsOn: [insights, queue]
  params: {
    location: location
    resourceNameFormat: resourceNameFormat
    alphavantageApiKey: alphavantageApiKey
  }
}
