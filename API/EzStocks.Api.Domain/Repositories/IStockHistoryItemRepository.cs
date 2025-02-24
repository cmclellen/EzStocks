namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockHistoryItemRepository
    {
        Task<IList<Domain.Entities.StockHistoryItem>> GetStockHistoryAsync(CancellationToken cancellationToken = default);
    }
}
