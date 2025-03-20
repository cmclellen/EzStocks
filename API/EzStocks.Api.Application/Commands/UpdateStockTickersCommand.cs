using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Commands
{
    public record UpdateStockTickersCommand : IRequest;

    public class UpdateStockTickersCommandHandler(
        ILogger<UpdateStockTickersCommandHandler> _logger,
        IStocksApiClient _stocksApiClient,
        IStockTickerRepository _stockTickerRepository,
        IUnitOfWork _unitOfWork) : IRequestHandler<UpdateStockTickersCommand>
    {
        public async Task Handle(UpdateStockTickersCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching all stock tickers...");
            int totalStockTickerCount = 0;
            string? cursor = null;
            for (int i = 0; ; i++)
            {
                using var _ = _logger.BeginScope(new Dictionary<string, object> { ["Page"] = i + 1 });
                _logger.LogDebug("Fetching stock tickers...");
                var getStockTickersResponse = await _stocksApiClient.GetStockTickersAsync(new GetStockTickersRequest(1000, cursor), cancellationToken);
                _logger.LogDebug("Successfully fetched {StockTickerPageCount} stock tickers", getStockTickersResponse.Count);
                totalStockTickerCount += getStockTickersResponse.Count;

                await _stockTickerRepository.UpsertAsync(getStockTickersResponse.Items.Select(i => new Domain.Entities.StockTicker { Symbol = i.Ticker }).ToList(), cancellationToken);

                //await _unitOfWork.CommitAsync(cancellationToken);

                if (string.IsNullOrEmpty(cursor = getStockTickersResponse.Cursor))
                {
                    break;
                }
            }
            _logger.LogDebug("Successfully fetched all {TotalStockTickerCount:n0} stock tickers", totalStockTickerCount);
        }
    }
}
