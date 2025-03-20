using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockTickerRepository(EzStockDbContext ezStockDbContext) : IStockTickerRepository
    {
        public async Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken)
        {   
            await ezStockDbContext.StockTickers.AddRangeAsync(stockTickers, cancellationToken);
        }
    }
}
