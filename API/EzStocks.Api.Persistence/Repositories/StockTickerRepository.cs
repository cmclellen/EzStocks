using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockTickerRepository(EzStockDbContext ezStockDbContext) : IStockTickerRepository
    {
        public Task UpsertAsync(IList<string> stockTickers, CancellationToken cancellationToken)
        {
            //ezStockDbContext.StockTickers.BulAdd
            throw new NotImplementedException();
        }
    }
}
