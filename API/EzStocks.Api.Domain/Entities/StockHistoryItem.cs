namespace EzStocks.Api.Domain.Entities
{
    public class StockHistoryItem
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid StockItemId { get; set; }

        public StockItem? StockItem { get; set; }
    }
}
