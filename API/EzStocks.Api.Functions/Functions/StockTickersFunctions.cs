using EzStocks.Api.Application.Commands;
using EzStocks.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AzStocks.Api.Functions.Functions
{
    public class StockTickersFunctions(
        ISender sender)
    {
        [Function(nameof(SearchStockTickers))]
        public async Task<IActionResult> SearchStockTickers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock-tickers/search")] HttpRequest req, string searchText, CancellationToken cancellationToken)
        {
            var result = await sender.Send(new SearchStockTickersQuery(searchText), cancellationToken);
            return new OkObjectResult(result);
        }

        [Function(nameof(CreateStockTicker))]
        public async Task<IActionResult> CreateStockTicker([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-tickers")] HttpRequest req, string? ticker = null, CancellationToken cancellationToken = default)
        {
            await sender.Send(new CreateStockTickerCommand(ticker), cancellationToken);
            return new AcceptedResult();
        }
    }
}
