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
            
            //stockPriceItemsToSave.Select(item => item.AsAtDate)
            //stockPriceItemRepository.GetByAsAtDatesAsync();
            foreach (var stockPriceItem in stockPriceItemsToSave)
            {
                await stockPriceItemRepository.CreateAsync(stockPriceItem, cancellationToken);
            }
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }

}
