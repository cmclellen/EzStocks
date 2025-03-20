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

        [Function(nameof(UpdateStockTickersTimer))]
        public async Task<IActionResult> UpdateStockTickersTimer([TimerTrigger("0 0 23 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateStockTickersCommand(), cancellationToken);
            return new OkResult();
        }

        [Function(nameof(UpdateStockTickers))]
        public async Task<IActionResult> UpdateStockTickers([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-tickers/update")] HttpRequest req, CancellationToken cancellationToken)
        {
            await sender.Send(new UpdateStockTickersCommand(), cancellationToken);         
            return new AcceptedResult();
        }
    }
}
