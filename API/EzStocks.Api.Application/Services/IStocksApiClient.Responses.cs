namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateOnly Date { get; set; }
        public decimal Close { get; set; }
    }

    public record GetStockPriceResponse(string Symbol, TimeZoneInfo TimeZone, List<OhlcvItem> OhlcvItems);

    public record SearchStockTickersResponse(IList<TickerSymbol> TickerSymbols, int Count, string? Cursor);

    public record TickerSymbol(string Symbol, string Name, string Region, string Currency);

    //public record TickerItem(string Ticker, string Name);

    //public record GetStockTickersResponse(List<TickerItem> Items, int Count, string? Cursor);
}
