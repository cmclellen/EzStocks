namespace EzStocks.Api.Application.Dtos
{
    public class StockTicker
    {
        public Guid? Id { get; set; }
        public required string Ticker { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
    }
}
