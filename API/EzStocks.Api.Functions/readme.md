# EZ Stock

## Setup the user secrets
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "AccountEndpoint=https://cosmos-ezstocks-dev-aue.documents.azure.com:443/;AccountKey=xxx;"
  },
  "PolygonIO": {
    "ApiKey": "yyy"
  },
  "APPLICATIONINSIGHTS_CONNECTION_STRING": "InstrumentationKey=zzz;IngestionEndpoint=https://australiaeast-0.in.applicationinsights.azure.com/;LiveEndpoint=https://australiaeast.livediagnostics.monitor.azure.com/;ApplicationId=ppp"
}
```

## Trigger fetching a stock price
```json
{ "Symbol": "MSFT" }
```

## Create a user
```json
{ 
  "UserId": "B8343EAF-6C20-4A40-BADA-168FF5CEEC89",
  "StockItems": 
  [
    {
      "Id": "d06d3bd1-3954-4f4d-7314-08dd5926f69d",
      "Symbol": "MSFT"
    },
    {
      "Id": "c50970be-5711-41a4-7315-08dd5926f69d",
      "Symbol": "AAPL"
    }
  ]
}
```