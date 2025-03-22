using EzStocks.Api.Application.Services;
using System.Text.Json.Nodes;

namespace EzStocks.Api.Infrastructure.Alphavantage.Mappers
{
    public interface ISearchForSymbolResponseMapper
    {
        SearchStockTickersResponse MapFromJson(string json);
    }

    public class SearchForSymbolResponseMapper : ISearchForSymbolResponseMapper
    {
        public SearchStockTickersResponse MapFromJson(string json)
        {
            var root = JsonNode.Parse(json)!;
            var bestMatches = root["bestMatches"]!;
            var builder = new Builders.TickerSymbolBuilder();
            var tickerSymbols = bestMatches.AsArray().Aggregate(new List<StockTicker>(), (acc, item) =>
            {
                var obj = item!.AsObject().AsEnumerable();
                foreach (var objItem in obj)
                {
                    switch (objItem.Key)
                    {
                        case "1. symbol":
                            builder.SetSymbol(objItem.Value!.GetValue<string>());
                            break;

                        case "2. name":
                            builder.SetName(objItem.Value!.GetValue<string>());
                            break;

                        case "4. region":
                            builder.SetRegion(objItem.Value!.GetValue<string>());
                            break;

                        case "7. timezone":
                            builder.SetTimeZone(objItem.Value!.GetValue<string>());
                            break;

                        case "8. currency":
                            builder.SetCurrency(objItem.Value!.GetValue<string>());
                            break;
                    }
                }
                acc.Add(builder.Build());
                return acc;
            });

            return new SearchStockTickersResponse(tickerSymbols, tickerSymbols.Count, null);
        }
    }
}
