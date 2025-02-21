namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockHistoryRepository
    {
        Task<IList<Domain.Entities.StockHistoryItem>> GetStockHistoryAsync(CancellationToken cancellationToken = default);
    }
}
