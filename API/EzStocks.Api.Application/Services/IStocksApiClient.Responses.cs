namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }

    public class GetStockPriceResponse
    {
        public List<OhlcvItem> OhlcvItems { get; set; } = new List<OhlcvItem>();
    }
}
