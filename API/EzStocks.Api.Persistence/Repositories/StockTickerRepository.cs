using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockTickerRepository(EzStockDbContext ezStockDbContext) : IStockTickerRepository
    {
        public async Task<IList<StockTicker>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await ezStockDbContext.StockTickers.ToListAsync(cancellationToken);
        }

        public async Task<IList<StockTicker>> GetBySymbolsAsync(IList<string> symbols, CancellationToken cancellationToken)
        {
            //await ezStockDbContext.StockTickers.AddRangeAsync(stockTickers, cancellationToken);
            IQueryable<StockTicker> query = ezStockDbContext.StockTickers;
            if (symbols is not null)
            {
                query = query.Where(si => symbols.Contains(si.Symbol));
            }
            return await query.ToListAsync();
        }

        public async Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken)
        {
            await ezStockDbContext.StockTickers.AddRangeAsync(stockTickers, cancellationToken);

            //var db = ezStockDbContext.Database;

            //var cosmosClient = db.GetCosmosClient();
            ////cosmosClient.ClientOptions.AllowBulkExecution = true;
            //var container = cosmosClient.GetContainer("EzStocks", nameof(Lookup));

            //List<Task> concurrentTasks = new List<Task>();
            //foreach(var stockTicker in stockTickers)
            //{
            //    //var tsk = await container.UpsertItemAsync(stockTicker, cancellationToken: cancellationToken);
            //    var tsk = await container.CreateItemAsync(stockTicker, cancellationToken: cancellationToken);
            //    break;
            //    //concurrentTasks.Add(tsk);
            //}
            //await Task.WhenAll(concurrentTasks);
        }
    }
}
