namespace EzStocks.Api.Application.Services
{
    public record GetStockPriceRequest(string Ticker);

    public record SearchStockTickersRequest(string SearchText, int Limit = 20, string? Cursor = null);
}
