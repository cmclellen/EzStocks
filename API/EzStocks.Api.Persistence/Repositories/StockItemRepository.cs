using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockItemRepository(Persistence.EzStockDbContext ezStockDbContext) : IStockItemRepository
    {
        public async Task<IList<StockItem>> GetStocksAsync(CancellationToken cancellation = default)
        {
            return await ezStockDbContext.StockItems.ToListAsync();
        }

        public async Task CreateStockAsync(Domain.Entities.StockItem stockItem, CancellationToken cancellation = default)
        {
            await ezStockDbContext.StockItems.AddAsync(stockItem, cancellation);
        }
    }
}
