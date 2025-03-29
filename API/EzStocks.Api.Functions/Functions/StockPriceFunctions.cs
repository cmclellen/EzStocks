using EzStocks.Api.Application.Commands;
using EzStocks.Api.Application.Queries;
using EzStocks.Api.Functions.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

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
            [ServiceBusOutput(QueueName.PopulateStockPrices, Connection = "ServiceBusConnection")]
            public required IEnumerable<PopulateStockPriceItemCommand> OutputEvents { get; set; }

            [HttpResult]
            public required IActionResult Result { get; set; }
        }

        [Function(nameof(PopulateStockPrice))]
        public async Task<PopulateStockPriceOutput> PopulateStockPrice(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices/populate")] HttpRequestData req, string? ticker = null, CancellationToken cancellationToken = default)
        {
            List<PopulateStockPriceItemCommand> commands = new List<PopulateStockPriceItemCommand>();
            if (ticker is not null)
            {
                commands.Add(new PopulateStockPriceItemCommand(ticker));
            }
            else
            {
                var populateStockPriceItemCommands = await GeneratePopulateStockPriceItemCommands(cancellationToken);
                commands.AddRange(populateStockPriceItemCommands);
            }
            return new PopulateStockPriceOutput
            {
                OutputEvents = commands,
                Result = new OkResult()
            };
        }

        [Function(nameof(PopulateStockPriceCommand))]
        public async Task<IActionResult> PopulateStockPriceCommand([ServiceBusTrigger(QueueName.PopulateStockPrices, Connection = "ServiceBusConnection")]
        PopulateStockPriceItemCommand populateStockPriceItemCommand, CancellationToken cancellationToken)
        {   
            await _sender.Send(populateStockPriceItemCommand, cancellationToken);
            return new OkResult();
        }

        private async Task<PopulateStockPriceItemCommand[]> GeneratePopulateStockPriceItemCommands(CancellationToken cancellationToken)
        {
            var allStockTickers = await _sender.Send(new GetStockTickersQuery(), cancellationToken);
            List<string> allTickers = allStockTickers.Select(item => item.Ticker).ToList();
            return allTickers.Select(ticker => new PopulateStockPriceItemCommand(ticker)).ToArray();
        }

        [Function(nameof(PopulateStockPricesTimer))]
        [ServiceBusOutput(QueueName.PopulateStockPrices, Connection = "ServiceBusConnection")]
        public async Task<PopulateStockPriceItemCommand[]> PopulateStockPricesTimer([TimerTrigger("0 30 20 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {            
            return await GeneratePopulateStockPriceItemCommands(cancellationToken);
        }

        [Function(nameof(GetStockPricesHistory))]
        public async Task<IActionResult> GetStockPricesHistory([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock-prices/history")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockHistory = await _sender.Send(new GetStocksHistoryQuery(), cancellationToken);
            return new OkObjectResult(stockHistory.Value);
        }
    }
}