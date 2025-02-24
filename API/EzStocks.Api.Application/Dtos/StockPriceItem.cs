namespace EzStocks.Api.Application.Dtos
{
    public class StockPriceItem
    {
        public required string Symbol { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDateUtc { get; set; }
    }
}
