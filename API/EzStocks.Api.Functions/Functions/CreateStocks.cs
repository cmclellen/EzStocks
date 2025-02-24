using EzStocks.Api.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AzStocks.Api.Functions.Functions
{
    public class CreateStocks(
        ISender sender)
    {
        [Function(nameof(CreateStocks))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stocks")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockItem>();
            await sender.Send(new CreateStockCommand(stockItem!), cancellationToken);
            return new CreatedResult();
        }
    }
}
