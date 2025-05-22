using EzStocks.Api.Application.Commands;
using EzStocks.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Ardalis.Result;
using DarkLoop.Azure.Functions.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace AzStocks.Api.Functions.Functions
{
    [FunctionAuthorize]
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
        public async Task<IActionResult> CreateStockTicker([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-tickers")] HttpRequest req, string ticker, string name, string color, CancellationToken cancellationToken = default)
        {
            await sender.Send(new CreateStockTickerCommand(ticker, name, color), cancellationToken);
            return new AcceptedResult();
        }

        [Function(nameof(DeleteStockTicker))]
        public async Task<IActionResult> DeleteStockTicker([HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "stock-tickers")] HttpRequest req, string ticker, string name, string color, CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new DeleteStockTickerCommand(ticker), cancellationToken);
            return result.IsNotFound() ? new NotFoundResult() : new NoContentResult();
        }

        [Function(nameof(UpdateStockTicker))]
        public async Task<IActionResult> UpdateStockTicker([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "stock-tickers")] HttpRequest req, string ticker, string name, string color, CancellationToken cancellationToken = default)
        {
            var result = await sender.Send(new UpdateStockTickerCommand(ticker, name, color), cancellationToken);
            return result.IsNotFound() ? new NotFoundResult() : new OkResult();
        }

        [Authorize(Policy = "AuthenticatedUser")]
        [Function(nameof(GetStockTickers))]
        public async Task<IActionResult> GetStockTickers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock-tickers")] HttpRequest req, CancellationToken cancellationToken = default)
        {
            string inputString = req.Query["Tickers"];
            List<string>? tickers = string.IsNullOrEmpty(inputString)? null : inputString.Split(',').ToList();
            var result = await sender.Send(new GetStockTickersCommand(tickers), cancellationToken);
            return new OkObjectResult(result);
        }
    }
}
