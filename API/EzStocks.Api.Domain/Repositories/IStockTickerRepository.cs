
using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockTickerRepository
    {
        Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken);
    }
}
