using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockRepository
    {
        Task<IList<StockItem>> GetStocksAsync(CancellationToken cancellation = default);
    }
}
