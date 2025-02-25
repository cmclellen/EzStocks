using AzStocks.Api.Functions.Functions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace EzStocks.Api.Functions.Functions
{
    public class StockPriceItemFunctions(ISender sender)
    {
        [Function(nameof(CreateStockPriceItem))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateStockPriceItem([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stockpriceitems")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockPriceItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockPriceItem>();
            await sender.Send(new Application.Commands.CreateStockPriceItemCommand(stockPriceItem!), cancellationToken);
            return new CreatedResult();
        }
    }
}
