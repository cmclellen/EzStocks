param location string = resourceGroup().location

param resourceNameFormat string

var queueNames = [
  'fetch-stock-prices'
]

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2024-01-01' = {
  name: format(resourceNameFormat, 'sbns')
  location: location
  sku: {
    name: 'Basic'
  }

  resource sbqueues 'queues' = [
    for queueName in queueNames: {
      name: queueName
      properties: {}
    }
  ]
}
