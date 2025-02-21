using EzStocks.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace AzStocks.Api.Functions.Functions
{
    public class GetStocks(
        ISender sender)
    {
        [Function(nameof(GetStocks))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stocks = await sender.Send(new GetStocksQuery(), cancellationToken);
            return new OkObjectResult(stocks);
        }
    }
}
