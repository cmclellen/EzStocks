param location string = resourceGroup().location

param resourceNameFormat string

@description('The name for the SQL API database')
param databaseName string = 'EzStocks'

resource keyVault 'Microsoft.KeyVault/vaults@2024-04-01-preview' existing = {
  name: format(resourceNameFormat, 'kv')
}

// @description('The name for the SQL API container')
// param containerName string = 'stockscontainer1'

resource account 'Microsoft.DocumentDB/databaseAccounts@2024-12-01-preview' = {
  name: format(resourceNameFormat, 'cosmos')
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    enableFreeTier: true
    consistencyPolicy: { defaultConsistencyLevel: 'Session' }
    databaseAccountOfferType: 'Standard'
    // Cheaper to go with serverless
    capacityMode: 'Serverless'
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
      // options: {
      //   throughput: 1000
      // }
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

resource cosmosdbConnectionString 'Microsoft.KeyVault/vaults/secrets@2024-04-01-preview' = {
  name: 'cosmosdb-connection-string'
  parent: keyVault
  properties: {
    value: account.listConnectionStrings().connectionStrings[0].connectionString
  }
}
