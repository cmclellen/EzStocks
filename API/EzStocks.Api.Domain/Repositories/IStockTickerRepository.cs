﻿
using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockTickerRepository
    {
        Task<IList<StockTicker>> GetByTickersAsync(IList<string>? tickers, CancellationToken cancellationToken);
        Task AddAsync(StockTicker stockTicker, CancellationToken cancellationToken);
    }
}
