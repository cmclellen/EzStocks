namespace EzStocks.Api.Application.Dtos
{
    public class StockPriceItem
    {
        public required string Ticker { get; set; }
        public decimal Close { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
