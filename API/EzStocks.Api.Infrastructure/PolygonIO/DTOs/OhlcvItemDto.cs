using System.Text.Json.Serialization;

namespace EzStocks.Api.Infrastructure.PolygonIO.DTOs
{
    public class OhlcvItemDto
    {
        public required decimal Close { get; set; }
        public required string Symbol { get; set; }
    }
}
