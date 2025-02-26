namespace EzStocks.Api.Application.Services
{
    public interface IStocksApiClient
    {
        Task<GetStockPriceResponse> GetStockPriceAsync(string symbol, CancellationToken cancellationToken);
    }
}
