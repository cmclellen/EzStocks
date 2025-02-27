using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record FetchStockPriceItemCommand(string Symbol) : IRequest;

    public class FetchStockPriceItemCommandHandler(
        IUnitOfWork unitOfWork,
        IStockPriceItemRepository stockPriceItemRepository,
        IStocksApiClient stocksApiClient) : IRequestHandler<FetchStockPriceItemCommand>
    {
        public async Task Handle(FetchStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            var getStockPriceResponse = await stocksApiClient.GetStockPriceAsync(new GetStockPriceRequest(request.Symbol), cancellationToken);

            var stockPriceItems = getStockPriceResponse.OhlcvItems.Select(item => new Domain.Entities.StockPriceItem
            {
                Close = item.Close,
                Symbol = request.Symbol,
                IanaTimeZoneId = getStockPriceResponse.TimeZone.Id,
                AsAtDate = item.Date
            }).ToList();

            var stockPriceItemsToSave = stockPriceItems.Take(3);

            var asAtDates = stockPriceItemsToSave.Select(item => item.AsAtDate).ToList();
            var existingItems = await stockPriceItemRepository.GetByAsAtDatesAsync(asAtDates, cancellationToken);
            foreach (var stockPriceItem in stockPriceItemsToSave)
            {
                var existingItem = existingItems.FirstOrDefault(item => item.AsAtDate == stockPriceItem.AsAtDate);
                if(existingItem is null)
                {
                    await stockPriceItemRepository.CreateAsync(stockPriceItem, cancellationToken);
                } 
                else
                {
                    existingItem.Close = stockPriceItem.Close;
                    await stockPriceItemRepository.UpdateAsync(existingItem, cancellationToken);
                }
            }
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }

}
