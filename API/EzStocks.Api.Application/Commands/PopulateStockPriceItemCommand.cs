using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EzStocks.Api.Application.Commands
{
    [DebuggerDisplay("{Ticker}")]
    public record PopulateStockPriceItemCommand(string Ticker, int? MaxItemCount = 3) : IRequest;

    public class PopulateStockPriceItemCommandHandler(
        ILogger<PopulateStockPriceItemCommandHandler> _logger,
        IUnitOfWork _unitOfWork,
        IStockPriceItemRepository _stockPriceItemRepository,
        IStocksApiClient _stocksApiClient) : IRequestHandler<PopulateStockPriceItemCommand>
    {
        public async Task Handle(PopulateStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            using var _ = _logger.BeginScope(new Dictionary<string, object> { ["Ticker"] = request.Ticker });

            _logger.LogDebug($"Retrieving stock prices...");
            var getStockPriceResponse = await _stocksApiClient.GetStockPriceAsync(new GetStockPriceRequest(request.Ticker), cancellationToken);
            _logger.LogInformation("Successfully retrieved stock price");

            var stockPriceItems = getStockPriceResponse.OhlcvItems.Select(item => new Domain.Entities.StockPriceItem
            {
                Close = item.Close,
                Ticker = request.Ticker,
                IanaTimeZoneId = getStockPriceResponse.TimeZone.Id,
                AsAtDate = item.Date
            }).ToList();

            var stockPriceItemsToSave = stockPriceItems.Take(request.MaxItemCount!.Value);

            _logger.LogDebug("Retrieving existing stock prices...");
            var asAtDates = stockPriceItemsToSave.Select(item => item.AsAtDate).ToList();
            var existingItems = await _stockPriceItemRepository.GetByAsAtDatesAsync(request.Ticker, asAtDates, cancellationToken);
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
                    existingItem.IanaTimeZoneId = stockPriceItem.IanaTimeZoneId;
                    await _stockPriceItemRepository.UpdateAsync(existingItem, cancellationToken);
                }
            }
            await _unitOfWork.CommitAsync(cancellationToken);
            _logger.LogInformation("Saved stock prices");
        }
    }
}
