using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockItemRepository(Persistence.EzStockDbContext ezStockDbContext) : IStockItemRepository
    {
        public async Task<IList<StockItem>> GetStocksAsync(CancellationToken cancellation = default)
        {
            await Task.CompletedTask;
            var data = new List<StockItem>{
                new StockItem{Id = Guid.NewGuid(), Code = "AAPL", Name = "Apple Inc."},
                new StockItem{Id = Guid.NewGuid(), Code = "MSFT", Name = "Microsoft Corp"},
            };
            return data;
        }

        public async Task CreateStockAsync(Domain.Entities.StockItem stockItem, CancellationToken cancellation = default)
        {
            await ezStockDbContext.AddAsync(stockItem, cancellation);
        }
    }
}
