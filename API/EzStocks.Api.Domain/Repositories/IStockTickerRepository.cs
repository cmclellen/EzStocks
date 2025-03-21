
using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockTickerRepository
    {
        Task<IList<StockTicker>> GetAllAsync(CancellationToken cancellationToken);
        Task<IList<StockTicker>> GetBySymbolsAsync(IList<string> symbols, CancellationToken cancellationToken);
        Task UpsertAsync(IList<StockTicker> stockTickers, CancellationToken cancellationToken);
    }
}
