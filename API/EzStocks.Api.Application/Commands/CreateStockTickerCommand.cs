using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Commands
{
    public record CreateStockTickerCommand(string Ticker) : IRequest;

    public class CreateStockTickersCommandHandler(
        ILogger<CreateStockTickersCommandHandler> _logger,
        IStocksApiClient _stocksApiClient,
        IStockTickerRepository _stockTickerRepository,
        IUnitOfWork _unitOfWork) : IRequestHandler<CreateStockTickerCommand>
    {
        public async Task Handle(CreateStockTickerCommand request, CancellationToken cancellationToken)
        {
            using var scopeTicker = _logger.BeginScope(new Dictionary<string, object> { ["Ticker"] = request.Ticker });

            _logger.LogDebug("Creating stock ticker...");

            var existingStockTicker = await _stockTickerRepository.GetByTickersAsync([request.Ticker], cancellationToken);
            if(existingStockTicker is null)
            {
                throw new Exception($"Stock ticker {request.Ticker} not found");
            }

            int totalStockTickerCount = 0;
            string? cursor = null;
            for (int i = 0; ; i++)
            {
                using var scopePage = _logger.BeginScope(new Dictionary<string, object> { ["Page"] = i + 1 });
                _logger.LogDebug("Populating stock tickers...");
                var getStockTickersResponse = await _stocksApiClient.SearchStockTickersAsync(new SearchStockTickersRequest(request.Ticker, Cursor: cursor), cancellationToken);
                _logger.LogDebug("Successfully populated {StockTickerPageCount} stock tickers", getStockTickersResponse.Count);
                totalStockTickerCount += getStockTickersResponse.Count;

                var stockTickersToAdd = getStockTickersResponse.StockTickers
                    .Where(i=> existingStockTicker.Any(e=>e.Ticker != i.Ticker))
                    .Select(i => new Domain.Entities.StockTicker { Ticker = i.Ticker, Name= i.Name, Color = "#000" })
                    .ToList();

                if(stockTickersToAdd.Any())
                {
                    await _stockTickerRepository.UpsertAsync(stockTickersToAdd, cancellationToken);

                    await _unitOfWork.CommitAsync(cancellationToken);
                }

                if (string.IsNullOrEmpty(cursor = getStockTickersResponse.Cursor))
                {
                    break;
                }
            }
            _logger.LogDebug("Successfully populated {TotalStockTickerCount:n0} stock ticker(s)", totalStockTickerCount);
        }
    }
}
