using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockTickerRepository(EzStockDbContext ezStockDbContext) : IStockTickerRepository
    {
        public async Task<IList<StockTicker>> GetByTickersAsync(IList<string>? tickers, CancellationToken cancellationToken)
        {
            var query = ezStockDbContext.StockTickers.AsQueryable();
            if(tickers is not null)
            {
                query = query.Where(si => tickers.Contains(si.Ticker));
            }
            IList<StockTicker> stockTickers = await query.ToListAsync();
            return stockTickers;
        }

        public async Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken)
        {
            await ezStockDbContext.StockTickers.AddRangeAsync(stockTickers, cancellationToken);
        }
    }
}
