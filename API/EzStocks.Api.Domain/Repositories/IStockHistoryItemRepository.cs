namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockHistoryItemRepository
    {
        Task<IList<Domain.Entities.StockHistoryItem>> GetAsync(CancellationToken cancellationToken = default);
    }
}
