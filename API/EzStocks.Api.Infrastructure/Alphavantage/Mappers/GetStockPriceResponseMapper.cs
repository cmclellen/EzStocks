using EzStocks.Api.Application.Services;
using System.Globalization;
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
            var metaDataNode = root["Meta Data"]!;
            var symbol = metaDataNode["2. Symbol"]!.GetValue<string>();
            var timeZone = GetTimeZone(metaDataNode);
            return new GetStockPriceResponse(symbol, timeZone, ParseTimeSeries(root));
        }

        private TimeZoneInfo GetTimeZone(JsonNode metaDataNode)
        {
            var tzText = metaDataNode["5. Time Zone"]!.GetValue<string>();
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(tzText);
            return tz;
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
                        Date = DateOnly.ParseExact(dateText, "yyyy-MM-dd", CultureInfo.InvariantCulture),
                        Close = decimal.Parse(closeNode.GetValue<string>())
                    };
                ohlcvItems.Add(ohlcvItem);
            }
            return ohlcvItems.OrderByDescending(i => i.Date).ToList();
        }
    }
}
