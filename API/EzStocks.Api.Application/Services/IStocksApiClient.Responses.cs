namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateTime Date { get; set; }
        public decimal Close { get; set; }
    }

    public record GetStockPriceResponse(string Symbol, TimeZoneInfo TimeZone, List<OhlcvItem> OhlcvItems);
}
