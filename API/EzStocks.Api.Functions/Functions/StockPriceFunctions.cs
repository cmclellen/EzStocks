using Azure.Messaging.ServiceBus;
using EzStocks.Api.Application.Commands;
using EzStocks.Api.Application.Json;
using EzStocks.Api.Application.Queries;
using EzStocks.Api.Domain.Utils;
using EzStocks.Api.Functions.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Functions.Functions
{
    public class StockPriceFunctions
    {
        private readonly ILogger<StockPriceFunctions> _logger;
        private readonly ISender _sender;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly IJsonSerializer _jsonSerializer;

        public StockPriceFunctions(
            ILogger<StockPriceFunctions> logger,
            ISender sender,
            ServiceBusClient serviceBusClient,
            IJsonSerializer jsonSerializer)
        {
            _logger = logger;
            _sender = sender;
            _serviceBusClient = serviceBusClient;
            _jsonSerializer = jsonSerializer;
        }

        [Function(nameof(CreateStockPrice))]
        public async Task<IActionResult> CreateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockPriceItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockPriceItem>();
            await _sender.Send(new Application.Commands.CreateStockPriceItemCommand(stockPriceItem!), cancellationToken);
            return new CreatedResult();
        }

        public class PopulateStockPriceOutput 
        {
            [HttpResult]
            public required IActionResult Result { get; set; }
        }

        [Function(nameof(PopulateStockPrice))]
        public async Task<PopulateStockPriceOutput> PopulateStockPrice(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stock-prices/populate")] HttpRequestData req, string? ticker = null, CancellationToken cancellationToken = default)
        {   
            if (ticker is not null)
            {
                var command = new PopulateStockPriceItemCommand(ticker);
                await SendCommands([command], cancellationToken);
            }
            else
            {
                await GeneratePopulateStockPriceItemCommands(cancellationToken);
            }
            return new PopulateStockPriceOutput
            {
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

        private async Task GeneratePopulateStockPriceItemCommands(CancellationToken cancellationToken)
        {
            var allStockTickers = await _sender.Send(new GetStockTickersQuery(), cancellationToken);
            List<string> allTickers = allStockTickers.Select(item => item.Ticker).ToList();
            var commands = allTickers.Select(ticker => new PopulateStockPriceItemCommand(ticker)).ToArray();

            await SendCommands(commands, cancellationToken);
        }

        private async Task SendCommands(PopulateStockPriceItemCommand[] commands, CancellationToken cancellationToken)
        {
            var utcNow = DateTimeProvider.Current.UtcNow;
            var sbSender = _serviceBusClient.CreateSender(QueueName.PopulateStockPrices);

            var serviceBusMessages = commands.Select((command, commandIndex) =>
            {
                var json = _jsonSerializer.Serialize(command);
                var serviceBusMessage = new ServiceBusMessage(json);
                serviceBusMessage.ScheduledEnqueueTime = utcNow.AddMinutes(commandIndex);
                return serviceBusMessage;
            }).ToList();

            await SendServiceBusMessagesAsBatch(sbSender, serviceBusMessages, cancellationToken);
        }

        private async Task SendServiceBusMessagesAsBatch(ServiceBusSender sbSender, List<ServiceBusMessage> serviceBusMessages, CancellationToken cancellationToken)
        {
            var batch = await sbSender.CreateMessageBatchAsync(cancellationToken);
            foreach (var serviceBusMessage in serviceBusMessages)
            {
                if (!batch.TryAddMessage(serviceBusMessage))
                {
                    throw new Exception("Unable to add message to ServiceBus message batch.");
                }
            }
            await sbSender.SendMessagesAsync(batch, cancellationToken);
        }

        [Function(nameof(PopulateStockPricesTimer))]
        public async Task PopulateStockPricesTimer([TimerTrigger("0 30 20 * * *")] TimerInfo timerInfo, FunctionContext context, CancellationToken cancellationToken)
        {            
            await GeneratePopulateStockPriceItemCommands(cancellationToken);
        }

        [Function(nameof(GetStockPricesHistory))]
        public async Task<IActionResult> GetStockPricesHistory([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stock-prices/history")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockHistory = await _sender.Send(new GetStocksHistoryQuery(), cancellationToken);
            return new OkObjectResult(stockHistory.Value);
        }
    }
}