namespace EzStocks.Api.Domain.Entities
{
    public class StockHistoryItem
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public required string Symbol { get; set; }
    }
}
