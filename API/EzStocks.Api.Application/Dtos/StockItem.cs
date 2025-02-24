namespace EzStocks.Api.Application.Dtos
{
    public class StockItem
    {
        public Guid? Id { get; set; }
        public required string Symbol { get; set; }
        public required string Name { get; set; }
    }
}
