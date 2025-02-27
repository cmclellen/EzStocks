using EzStocks.Api.Application.Services;
using System.Globalization;
using System.Runtime.InteropServices.JavaScript;
using System.Text.Json.Nodes;

namespace EzStocks.Api.Infrastructure.Alphavantage.Mappers
{
    public interface IGetStockPriceResponseMapper
    {
        GetStockPriceResponse MapFromJson(string json);
    }

    public class GetStockPriceResponseMapper : IGetStockPriceResponseMapper
    {
        public GetStockPriceResponse MapFromJson(string json)
        {
            var root = JsonNode.Parse(json)!;
            return new GetStockPriceResponse("IBM", "US/Eastern", ParseTimeSeries(root));
        }

        private List<OhlcvItem> ParseTimeSeries(JsonNode node)
        {
            var timeSeries = node["Time Series (Daily)"]!;
            List<OhlcvItem> ohlcvItems = new List<OhlcvItem>();
            foreach (var timeSeriesItem in timeSeries.AsObject().AsEnumerable())
            {
                var dateText = timeSeriesItem.Key;
                var closeNode = timeSeriesItem.Value!["4. close"]!;
                var ohlcvItem =
                    new OhlcvItem
                    {
                        Date = DateTime.ParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Close = decimal.Parse(closeNode.GetValue<string>())
                    };
                ohlcvItems.Add(ohlcvItem);
            }
            return ohlcvItems.OrderByDescending(i => i.Date).ToList();
        }
    }
}
