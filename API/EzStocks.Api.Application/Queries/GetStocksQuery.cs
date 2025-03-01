using AutoMapper;
using EzStocks.Api.Application.Observability;
using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Queries
{
    public record GetStocksQuery : IRequest<IList<Dtos.StockItem>>;

    public class GetStocksQueryHandler(
        ILogger<GetStocksQueryHandler> _logger,
        IMapper _mapper,
        IStockItemRepository _stockRepository) : IRequestHandler<GetStocksQuery, IList<Dtos.StockItem>>
    {
        public async Task<IList<Dtos.StockItem>> Handle(GetStocksQuery request, CancellationToken cancellationToken)
        {
            using var _ = Traces.DefaultSource.StartActivity("GetStockQuery");

            using var _1 = _logger.BeginScope(new Dictionary<string, object> { ["CraigValue"] = "here" });

            _logger.LogDebug("Retrieving stock items...");
            var stockEntities = await _stockRepository.GetStocksAsync(cancellationToken);
            _logger.LogInformation("Retrieved {StockItemCount} stock items", stockEntities.Count);

            IList<Dtos.StockItem> stockItems = stockEntities
                .Select(_mapper.Map<StockItem, Dtos.StockItem>)
                .ToList();
            return stockItems;
        }
    }
}
