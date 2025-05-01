// @description('Name of the project or solution')
// @minLength(3)
// @maxLength(37)
// param projectName string = 'EzStocks'

// @description('The location where the AAD B2C Directory will be deployed.')
// @allowed([
//   'global'
// ])
// param region string = 'global'

// @description('The name of the SKU for the AAD B2C Directory.')
// @allowed([
//   'Standard'
//   'PremiumP1'
// ])
// param skuName string = 'Standard'

// @description('The tier of the SKU for the AAD B2C Directory.')
// param skuTier string = 'A0'

// @description('The country code for the tenant.')
// param countryCode string = 'AU'

// @description('The display name for the AAD B2C Directory.')
// param displayName string = 'EzStocks'

// @description('Resource tags')
// param resourceTags object = {}

// var directoryName = toLower('${projectName}.onmicrosoft.com')

// resource AzAdB2c 'Microsoft.AzureActiveDirectory/b2cDirectories@2021-04-01' = {
//   name: directoryName
//   location: region
//   tags: resourceTags
//   sku: {
//     name: skuName
//     tier: skuTier
//   }
//   properties: {
//     createTenantProperties: {
//       countryCode: countryCode
//       displayName: displayName
//     }
//   }
// }

// output directoryId string = AzAdB2c.id
// output directoryLocation string = AzAdB2c.location
// output tenantId string = AzAdB2c.properties.tenantId
