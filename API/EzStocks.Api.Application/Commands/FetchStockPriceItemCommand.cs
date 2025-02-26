using EzStocks.Api.Application.Services;
using MediatR;

namespace EzStocks.Api.Application.Commands
{
    public record FetchStockPriceItemCommand : IRequest;

    public class FetchStockPriceItemCommandHandler(IStocksApiClient stocksApiClient) : IRequestHandler<FetchStockPriceItemCommand>
    {
        public async Task Handle(FetchStockPriceItemCommand request, CancellationToken cancellationToken)
        {
            await stocksApiClient.GetStockPriceAsync("MSFT", cancellationToken);
        }
    }

}
