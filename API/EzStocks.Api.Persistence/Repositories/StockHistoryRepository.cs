using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class StockHistoryRepository : IStockHistoryRepository
    {
        public async Task<IList<Domain.Entities.StockHistoryItem>> GetStockHistoryAsync(CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;
            var data = new List<Domain.Entities.StockHistoryItem> { };
            return data;
        }
    }
}
