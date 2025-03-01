﻿using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Commands
{
    public record FetchStockPriceItemCommand(string Symbol) : IRequest;

    public class FetchStockPriceItemCommandHandler(
        ILogger<FetchStockPriceItemCommandHandler> _logger,
        IUnitOfWork _unitOfWork,
        IStockPriceItemRepository _stockPriceItemRepository,
        IStocksApiClient _stocksApiClient) : IRequestHandler<FetchStockPriceItemCommand>
    {
        public async Task Handle(FetchStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            _logger.LogDebug("Fetching stock prices for {Symbol}...", request.Symbol);
            var getStockPriceResponse = await _stocksApiClient.GetStockPriceAsync(new GetStockPriceRequest(request.Symbol), cancellationToken);
            _logger.LogInformation("Successfully fetched stock prices for {Symbol}", request.Symbol);

            var stockPriceItems = getStockPriceResponse.OhlcvItems.Select(item => new Domain.Entities.StockPriceItem
            {
                Close = item.Close,
                Symbol = request.Symbol,
                IanaTimeZoneId = getStockPriceResponse.TimeZone.Id,
                AsAtDate = item.Date
            }).ToList();

            var stockPriceItemsToSave = stockPriceItems.Take(3);

            _logger.LogDebug("Retrieving existing stock prices for {Symbol}...", request.Symbol);
            var asAtDates = stockPriceItemsToSave.Select(item => item.AsAtDate).ToList();
            var existingItems = await _stockPriceItemRepository.GetByAsAtDatesAsync(asAtDates, cancellationToken);
            _logger.LogInformation("Found {HistoricStockPriceCount} existing stock prices for {Symbol}", existingItems.Count, request.Symbol);


            _logger.LogDebug("Saving stock prices for {Symbol}...", request.Symbol);
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
            _logger.LogInformation("Saved stock prices for {Symbol}", request.Symbol);
        }
    }

}
