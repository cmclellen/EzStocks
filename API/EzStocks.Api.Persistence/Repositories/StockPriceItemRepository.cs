
using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{

    public class StockPriceItemRepository(EzStockDbContext ezStockDbContext) : Domain.Repositories.IStockPriceItemRepository
    {
        public async Task CreateAsync(Domain.Entities.StockPriceItem stockPriceItem, CancellationToken cancellationToken)
        {
            await ezStockDbContext.StockPriceItems.AddAsync(stockPriceItem, cancellationToken);
        }

        public async Task<IList<Domain.Entities.StockPriceItem>> GetByAsAtDatesAsync(string symbol, IList<DateOnly> asAtDates, CancellationToken cancellationToken)
        {
            return await ezStockDbContext.StockPriceItems.Where(item => item.Ticker == symbol && asAtDates.Contains(item.AsAtDate)).ToListAsync(cancellationToken);
        }

        public async Task<IList<Domain.Entities.StockPriceItem>> GetByTickersAsync(List<string> tickers, CancellationToken cancellationToken)
        {
            return await ezStockDbContext.StockPriceItems.Where(item => tickers.Contains(item.Ticker)).ToListAsync(cancellationToken);
        }

        public Task UpdateAsync(StockPriceItem stockPriceItem, CancellationToken cancellationToken)
        {
            var entry = ezStockDbContext.StockPriceItems.Entry(stockPriceItem);
            entry.State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
