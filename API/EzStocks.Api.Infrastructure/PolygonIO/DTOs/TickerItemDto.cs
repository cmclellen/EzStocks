using System.Text.Json.Serialization;

namespace EzStocks.Api.Infrastructure.PolygonIO.DTOs
{
    public class TickerItemDto
    {
        public required string Ticker { get; set; }
        public required string Name { get; set; }
        public required string Locale { get; set; }
        [JsonPropertyName("currency_name")]
        public required string CurrencyName { get; set; }
    }
}
