namespace EzStocks.Api.Domain.Entities
{
    public class StockPriceItem : Lookup
    {
        public required string Symbol { get; set; }
        public decimal Close { get; set; }
        public DateOnly AsAtDate { get; set; }
        public required string IanaTimeZoneId { get; set; }
    }
}
