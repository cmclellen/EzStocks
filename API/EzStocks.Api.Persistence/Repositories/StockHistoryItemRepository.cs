using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockHistoryItemRepository : IStockHistoryItemRepository
    {
        public async Task<IList<Domain.Entities.StockHistoryItem>> GetAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            var data = new List<Domain.Entities.StockHistoryItem> { };
            return data;
        }
    }
}
