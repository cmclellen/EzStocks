using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockItemRepository
    {
        Task<IList<StockItem>> GetStocksAsync(CancellationToken cancellation = default);
        Task CreateStockAsync(Domain.Entities.StockItem stockItem, CancellationToken cancellation = default);
    }
}
