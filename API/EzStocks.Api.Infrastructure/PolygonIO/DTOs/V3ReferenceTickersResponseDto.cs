namespace EzStocks.Api.Infrastructure.PolygonIO.DTOs
{
    public class V3ReferenceTickersResponseDto
    {
        public List<TickerItemDto> Results { get; set; } = new List<TickerItemDto> ();
    }
}
