using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace EzStocks.Api.Functions.Functions
{
    public class StockPriceFunctions(ISender sender)
    {
        [Function(nameof(CreateStockPrice))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateStockPrice([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stockprices")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockPriceItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockPriceItem>();
            await sender.Send(new Application.Commands.CreateStockPriceItemCommand(stockPriceItem!), cancellationToken);
            return new CreatedResult();
        }
    }
}
