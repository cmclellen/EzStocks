using EzStocks.Api.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
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

        [Function(nameof(PopulateStockPrice))]
        public async Task<IActionResult> PopulateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices/populate")] HttpRequest req, CancellationToken cancellationToken)
        {
            await _sender.Send(new Application.Commands.PopulateStockPriceItemCommand("MSFT"), cancellationToken);
            return new OkResult();
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
            var stockItems = await _sender.Send(new Application.Queries.GetStocksQuery(), cancellationToken);
            List<string> symbols = stockItems.Select(item => item.Symbol).ToList();
            return symbols.Select(symbol => new PopulateStockPriceItemCommand(symbol)).ToArray();
        }
    }
}
