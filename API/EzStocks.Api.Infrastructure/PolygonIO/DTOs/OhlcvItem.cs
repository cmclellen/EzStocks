using System.Text.Json.Serialization;

namespace EzStocks.Api.Infrastructure.PolygonIO.DTOs
{
    public class OhlcvItem
    {
        public required decimal Close { get; set; }
        public required string Symbol { get; set; }
    }
}
