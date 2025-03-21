using AutoMapper;
using EzStocks.Api.Application.Observability;
using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksQuery() : IRequest<IList<Dtos.StockTicker>>;

    public class GetStocksQueryHandler(
        ILogger<GetStocksQueryHandler> _logger,
        IMapper _mapper,
        IStockTickerRepository _stockTickerRepository) : IRequestHandler<GetStocksQuery, IList<Dtos.StockTicker>>
    {
        public async Task<IList<Dtos.StockTicker>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            using var _ = Traces.DefaultSource.StartActivity("GetStockQuery");

            _logger.LogDebug("Retrieving stock items...");
            var stockEntities = await _stockTickerRepository.GetBySymbolsAsync(null, cancellationToken);
            _logger.LogInformation("Retrieved {StockItemCount} stock items", stockEntities.Count);

            IList<Dtos.StockTicker> stockItems = stockEntities
                .Select(_mapper.Map<StockTicker, Dtos.StockTicker>)
                .ToList();
            return stockItems;
        }
    }
}
