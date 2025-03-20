namespace EzStocks.Api.Domain.Entities
{
    public class StockTicker : Lookup
    {
        public required string Symbol { get; set; }
    }
}
