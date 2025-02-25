namespace EzStocks.Api.Persistence.Repositories
{

    public class StockPriceItemRepository(EzStockDbContext ezStockDbContext) : Domain.Repositories.IStockPriceItemRepository
    {
        public async Task CreateAsync(Domain.Entities.StockPriceItem stockPriceItem, CancellationToken cancellationToken)
        {
            await ezStockDbContext.StockPriceItems.AddAsync(stockPriceItem, cancellationToken);
        }
    }
}
