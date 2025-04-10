﻿
using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockPriceItemRepository
    {
        Task CreateAsync(Domain.Entities.StockPriceItem stockPriceItem, CancellationToken cancellationToken);
        Task<IList<Domain.Entities.StockPriceItem>> GetByAsAtDatesAsync(string symbol, IList<DateOnly> asAtDates, CancellationToken cancellationToken);
        Task<IList<Domain.Entities.StockPriceItem>> GetByTickersAsync(List<string> tickers, CancellationToken cancellationToken);
        Task UpdateAsync(StockPriceItem stockPriceItem, CancellationToken cancellationToken);
    }
}
