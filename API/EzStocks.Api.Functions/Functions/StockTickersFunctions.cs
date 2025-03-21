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
            var result = await sender.Send(new SearchStocksQuery(searchText), cancellationToken);
            return new OkObjectResult(result);
        }

        [Function(nameof(PopulateStockTickersTimer))]
        public async Task<IActionResult> PopulateStockTickersTimer([TimerTrigger("0 0 23 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {
            await sender.Send(new PopulateStockTickersCommand(), cancellationToken);
            return new OkResult();
        }

        [Function(nameof(PopulateStockTickers))]
        public async Task<IActionResult> PopulateStockTickers([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-tickers/populate")] HttpRequest req, string? ticker = null, CancellationToken cancellationToken = default)
        {
            await sender.Send(new PopulateStockTickersCommand(ticker), cancellationToken);
            return new AcceptedResult();
        }
    }
}
