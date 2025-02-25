namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockPriceItemRepository
    {
        Task CreateAsync(Domain.Entities.StockPriceItem stockPriceItem, CancellationToken cancellationToken);
    }
}
