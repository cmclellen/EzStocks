using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Commands
{
    public record FetchStockPriceItemCommand(string Symbol, int? MaxItemCount = 3) : IRequest;

    public class FetchStockPriceItemCommandHandler(
        ILogger<FetchStockPriceItemCommandHandler> _logger,
        IUnitOfWork _unitOfWork,
        IStockPriceItemRepository _stockPriceItemRepository,
        IStocksApiClient _stocksApiClient) : IRequestHandler<FetchStockPriceItemCommand>
    {
        public async Task Handle(FetchStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            using var _ = _logger.BeginScope(new Dictionary<string, object> { ["Symbol"] = request.Symbol });

            _logger.LogDebug($"Fetching stock prices...");
            var getStockPriceResponse = await _stocksApiClient.GetStockPriceAsync(new GetStockPriceRequest(request.Symbol), cancellationToken);
            _logger.LogInformation("Successfully fetched stock prices");

            var stockPriceItems = getStockPriceResponse.OhlcvItems.Select(item => new Domain.Entities.StockPriceItem
            {
                Close = item.Close,
                Symbol = request.Symbol,
                IanaTimeZoneId = getStockPriceResponse.TimeZone.Id,
                AsAtDate = item.Date
            }).ToList();

            var stockPriceItemsToSave = stockPriceItems.Take(request.MaxItemCount!.Value);

            _logger.LogDebug("Retrieving existing stock prices...");
            var asAtDates = stockPriceItemsToSave.Select(item => item.AsAtDate).ToList();
            var existingItems = await _stockPriceItemRepository.GetByAsAtDatesAsync(request.Symbol, asAtDates, cancellationToken);
            _logger.LogInformation("Found {HistoricStockPriceCount} existing stock prices", existingItems.Count);

            _logger.LogDebug("Saving stock prices...");
            foreach (var stockPriceItem in stockPriceItemsToSave)
            {
                var existingItem = existingItems.FirstOrDefault(item => item.AsAtDate == stockPriceItem.AsAtDate);
                if(existingItem is null)
                {
                    await _stockPriceItemRepository.CreateAsync(stockPriceItem, cancellationToken);
                } 
                else if(existingItem.Close != stockPriceItem.Close)
                {
                    existingItem.Close = stockPriceItem.Close;
                    await _stockPriceItemRepository.UpdateAsync(existingItem, cancellationToken);
                }
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Saved stock prices");
        }
    }

}
