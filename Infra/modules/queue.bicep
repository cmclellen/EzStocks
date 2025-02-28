param location string

param resourceNameFormat string

var queueNames = [
  'fetch-stock-prices'
]

resource serviceBusNamespace 'Microsoft.ServiceBus/namespaces@2022-10-01-preview' = {
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
