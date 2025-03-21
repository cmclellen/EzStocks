
using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockTickerRepository
    {
        Task<StockTicker?> GetBySymbolAsync(string symbol, CancellationToken cancellationToken);
        Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken);
    }
}
