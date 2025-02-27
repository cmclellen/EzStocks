using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record FetchStockPriceItemCommand : IRequest;

    public class FetchStockPriceItemCommandHandler(
        IUnitOfWork unitOfWork,
        IStockPriceItemRepository stockPriceItemRepository,
        IStocksApiClient stocksApiClient) : IRequestHandler<FetchStockPriceItemCommand>
    {
        public async Task Handle(FetchStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            var symbol = "MSFT";
            var getStockPriceResponse = await stocksApiClient.GetStockPriceAsync(new GetStockPriceRequest(symbol), cancellationToken);

            var stockPriceItems = getStockPriceResponse.OhlcvItems.Select(item => new Domain.Entities.StockPriceItem
            {
                Close = item.Close,
                Symbol = symbol,
                AsAtDateUtc = item.Date
            }).ToList();

            foreach(var stockPriceItem in stockPriceItems.Take(3))
            {
                await stockPriceItemRepository.CreateAsync(stockPriceItem, cancellationToken);
            }
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }

}
