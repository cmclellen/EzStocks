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
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stockprices")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockPriceItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockPriceItem>();
            await _sender.Send(new Application.Commands.CreateStockPriceItemCommand(stockPriceItem!), cancellationToken);
            return new CreatedResult();
        }

        //[Function(nameof(FetchStockPrice))]
        //public async Task<IActionResult> FetchStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stockprices/fetch")] HttpRequest req, CancellationToken cancellationToken)
        //{
        //    await sender.Send(new Application.Commands.FetchStockPriceItemCommand("MSFT"), cancellationToken);
        //    return new OkResult();
        //}

        [Function(nameof(FetchStockPrice))]
        public async Task<IActionResult> FetchStockPrice([ServiceBusTrigger("fetch-stock-prices", Connection = "ServiceBusConnection")]
        FetchStockPriceItemCommand fetchStockPriceItemCommand, CancellationToken cancellationToken)
        {   
            await _sender.Send(fetchStockPriceItemCommand, cancellationToken);
            return new OkResult();
        }

        [Function(nameof(FetchStockPricesTimer))]
        [ServiceBusOutput("fetch-stock-prices", Connection = "ServiceBusConnection")]
        public async Task<FetchStockPriceItemCommand[]> FetchStockPricesTimer([TimerTrigger("0 30 20 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {
            var stockItems = await _sender.Send(new Application.Queries.GetStocksQuery(), cancellationToken);
            List<string> symbols = stockItems.Select(item => item.Symbol).ToList();
            return symbols.Select(symbol => new FetchStockPriceItemCommand(symbol)).ToArray();
        }
    }
}
