namespace EzStocks.Api.Application.Services
{
    public record GetStockPriceRequest(string Symbol);

    public record SearchForSymbolRequest(string SearchText);

    public record GetStockTickersRequest(int Limit = 100, string? Cursor = null);
}
