using System.Text.Json.Serialization;

namespace EzStocks.Api.Infrastructure.PolygonIO.DTOs
{
    public class V3ReferenceTickersResponseDto
    {
        public List<TickerItemDto> Results { get; set; } = new List<TickerItemDto> ();

        public int Count { get; set; }

        [JsonPropertyName("next_url")]
        public string? NextUrl { get; set; }
    }
}
