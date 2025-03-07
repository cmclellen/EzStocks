using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockItemRepository(Persistence.EzStockDbContext ezStockDbContext) : IStockItemRepository
    {
        public async Task<IList<StockItem>> GetBySymbolsAsync(IList<string>? symbols, CancellationToken cancellation = default)
        {
            IQueryable<StockItem> query = ezStockDbContext.StockItems;
            if(symbols is not null)
            {
                query = query.Where(si => symbols.Contains(si.Symbol));
            }
            return await query.ToListAsync();
        }

        public async Task CreateAsync(Domain.Entities.StockItem stockItem, CancellationToken cancellation = default)
        {
            await ezStockDbContext.StockItems.AddAsync(stockItem, cancellation);
        }
    }
}
