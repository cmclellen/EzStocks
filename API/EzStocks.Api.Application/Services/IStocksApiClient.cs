namespace EzStocks.Api.Application.Services
{
    public interface IStocksApiClient
    {
        Task GetStockPriceAsync(string symbol, CancellationToken cancellationToken);
    }
}
