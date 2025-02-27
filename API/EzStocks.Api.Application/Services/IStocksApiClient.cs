namespace EzStocks.Api.Application.Services
{
    public interface IStocksApiClient
    {
        Task<GetStockPriceResponse> GetStockPriceAsync(GetStockPriceRequest request, CancellationToken cancellationToken);
    }
}
