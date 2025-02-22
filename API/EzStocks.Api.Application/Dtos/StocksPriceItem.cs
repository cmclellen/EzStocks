namespace EzStocks.Api.Application.Dtos
{
    public class StocksPriceItem
    {
        public DateOnly CreatedDate { get; set; }
        public required Dictionary<string, decimal> Stocks { get; set; }
    }
}
