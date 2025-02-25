namespace EzStocks.Api.Domain.Entities
{
    public class StockPriceItem : Lookup
    {
        public required string Symbol { get; set; }
        public decimal Close { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
