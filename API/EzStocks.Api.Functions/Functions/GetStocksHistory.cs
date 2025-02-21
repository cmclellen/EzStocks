using EzStocks.Api.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace EzStocks.Api.Functions.Functions
{
    public class GetStocksHistory(
        ISender sender)
    {
        [Function(nameof(GetStocksHistory))]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks/history")] HttpRequest req, CancellationToken cancellationToken)
        {
            var stockHistory = await sender.Send(new GetStocksHistoryQuery(), cancellationToken);
            return new OkObjectResult(stockHistory);
        }
    }
}

