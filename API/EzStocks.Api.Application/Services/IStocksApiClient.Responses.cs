namespace EzStocks.Api.Application.Services
{
    public class OhlcvItem
    {
        public DateOnly Date { get; set; }
        public decimal Close { get; set; }
    }

    public record GetStockPriceResponse(string Symbol, TimeZoneInfo TimeZone, List<OhlcvItem> OhlcvItems);

    public record SearchForSymbolResponse(IList<TickerSymbol> Symbols);

    public record TickerSymbol(string Symbol, string Name, string Region, string TimeZone, string Currency);
}
