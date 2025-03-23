using EzStocks.Api.Application.Commands;
using EzStocks.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace EzStocks.Api.Functions.Functions
{
    public class StockPriceFunctions(
        ILogger<StockPriceFunctions> _logger,
        ISender _sender)
    {
        [Function(nameof(CreateStockPrice))]
        public async Task<IActionResult> CreateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockPriceItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockPriceItem>();
            await _sender.Send(new Application.Commands.CreateStockPriceItemCommand(stockPriceItem!), cancellationToken);
            return new CreatedResult();
        }

        public class PopulateStockPriceOutput 
        {
            [ServiceBusOutput("populate-stock-prices", Connection = "ServiceBusConnection")]
            public IEnumerable<PopulateStockPriceItemCommand> OutputEvents { get; set; }

            [HttpResult]
            public IActionResult Result { get; set; }
        }

        [Function(nameof(PopulateStockPrice))]
        public async Task<PopulateStockPriceOutput> PopulateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices/populate")] HttpRequestData req, string? ticker = null, CancellationToken cancellationToken = default)
        {
            //HttpResponseData response = req.CreateResponse(HttpStatusCode.Accepted);
            
            List<PopulateStockPriceItemCommand> commands = new List<PopulateStockPriceItemCommand>();
            if (ticker is not null)
            {
                commands.Add(new Application.Commands.PopulateStockPriceItemCommand(ticker));
            }
            else
            {
                var allStockTickers = await _sender.Send(new Application.Queries.GetStockTickersQuery(), cancellationToken);
                List<string> allTickers = allStockTickers.Select(item => item.Ticker).ToList();
                commands.AddRange(allTickers.Select(ticker => new PopulateStockPriceItemCommand(ticker)).ToList());
            }
            return new PopulateStockPriceOutput
            {
                OutputEvents = commands,
                Result = new OkResult()
            };
        }

        [Function(nameof(PopulateStockPriceCommand))]
        public async Task<IActionResult> PopulateStockPriceCommand([ServiceBusTrigger("populate-stock-prices", Connection = "ServiceBusConnection")]
        PopulateStockPriceItemCommand populateStockPriceItemCommand, CancellationToken cancellationToken)
        {   
            await _sender.Send(populateStockPriceItemCommand, cancellationToken);
            return new OkResult();
        }

        [Function(nameof(PopulateStockPricesTimer))]
        [ServiceBusOutput("populate-stock-prices", Connection = "ServiceBusConnection")]
        public async Task<PopulateStockPriceItemCommand[]> PopulateStockPricesTimer([TimerTrigger("0 30 20 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {            
            var allStockTickers = await _sender.Send(new Application.Queries.GetStockTickersQuery(), cancellationToken);
            List<string> allTickers = allStockTickers.Select(item => item.Ticker).ToList();
            return allTickers.Select(ticker => new PopulateStockPriceItemCommand(ticker)).ToArray();
        }

        [Function(nameof(GetStockPricesHistory))]
        public async Task<IActionResult> GetStockPricesHistory([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock-prices/history")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockHistory = await _sender.Send(new GetStocksHistoryQuery(), cancellationToken);
            return new OkObjectResult(stockHistory.Value);
        }
    }
}