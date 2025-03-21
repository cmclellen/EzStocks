using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockTickerRepository(EzStockDbContext ezStockDbContext) : IStockTickerRepository
    {
        public Task<StockTicker?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken)
        {
            return ezStockDbContext.StockTickers.FirstOrDefaultAsync(si => symbol == si.Symbol);
        }

        public async Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken)
        {
            await ezStockDbContext.StockTickers.AddRangeAsync(stockTickers, cancellationToken);
        }
    }
}
