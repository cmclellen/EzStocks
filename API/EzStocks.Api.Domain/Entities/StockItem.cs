namespace EzStocks.Api.Domain.Entities
{
    public class StockItem : Lookup
    {   
        public required string Symbol { get; set; }
        public required string Name { get; set; }
        public required string Color { get; set; }
    }
}
