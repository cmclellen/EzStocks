namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }

    public record GetStockPriceResponse(string Symbol, string TimeZone, List<OhlcvItem> OhlcvItems);
}
