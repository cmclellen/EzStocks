namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateOnly Date { get; set; }
        public decimal Close { get; set; }
    }

    public record GetStockPriceResponse(string Ticker, TimeZoneInfo TimeZone, List<OhlcvItem> OhlcvItems);

    public record SearchStockTickersResponse(IList<StockTicker> StockTickers, int Count, string? Cursor);

    public record StockTicker(string Ticker, string Name, string Region, string Currency);
}
