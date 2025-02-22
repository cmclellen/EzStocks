param location string

param resourceNameFormat string

@description('The name for the SQL API database')
param databaseName string = 'EzStocks'

// @description('The name for the SQL API container')
// param containerName string = 'stockscontainer1'

resource account 'Microsoft.DocumentDB/databaseAccounts@2024-11-15' = {
  name: format(resourceNameFormat, 'cosmos')
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    enableFreeTier: true
    consistencyPolicy: { defaultConsistencyLevel: 'Session' }
    databaseAccountOfferType: 'Standard'
    locations: [
      {
        locationName: location
      }
    ]
    enableAutomaticFailover: false
  }

  resource database 'sqlDatabases' = {
    name: databaseName
    properties: {
      resource: {
        id: databaseName
      }
      options: {
        throughput: 1000
      }
    }

    //   resource container 'containers' = {
    //     name: containerName
    //     properties: {
    //       resource: {
    //         id: containerName
    //         partitionKey: {
    //           paths: [
    //             '/myPartitionKey'
    //           ]
    //           kind: 'Hash'
    //         }
    //         indexingPolicy: {
    //           indexingMode: 'consistent'
    //           includedPaths: [
    //             {
    //               path: '/*'
    //             }
    //           ]
    //           excludedPaths: [
    //             {
    //               path: '/_etag/?'
    //             }
    //           ]
    //         }
    //       }
    //     }
    //   }
  }
}
