namespace EzStocks.Api.Application.Services
{
    public record GetStockPriceRequest(string Symbol);

    public record SearchStockTickersRequest(string SearchText, int Limit = 100, string? Cursor = null);
}
