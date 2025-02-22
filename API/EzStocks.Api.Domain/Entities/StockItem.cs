namespace EzStocks.Api.Domain.Entities
{
    public class StockItem
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public required string Name { get; set; }
    }
}
