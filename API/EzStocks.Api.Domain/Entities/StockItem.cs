namespace EzStocks.Api.Domain.Entities
{
    public class StockItem
    {
        public Guid? Id { get; set; }
        public string Type { get; set; } = nameof(StockItem);
        public required string Symbol { get; set; }
        public required string Name { get; set; }
    }
}
