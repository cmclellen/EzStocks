//using EzStocks.Api.Application.Commands;
//using EzStocks.Api.Application.Queries;
//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Azure.Functions.Worker;

//namespace AzStocks.Api.Functions.Functions
//{
//    public class StockFunctions(
//        ISender sender)
//    {
//        [Function(nameof(GetStocks))]
//        public async Task<IActionResult> GetStocks([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "stocks")] HttpRequest req, CancellationToken cancellationToken)
//        {
//            var stocks = await sender.Send(new GetStocksQuery(), cancellationToken);
//            return new OkObjectResult(stocks);
//        }

//        //[Function(nameof(CreateStock))]
//        //public async Task<IActionResult> CreateStock([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stocks")] HttpRequest req, CancellationToken cancellationToken)
//        //{
//        //    var stockItem = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.StockTicker>();
//        //    await sender.Send(new CreateStockCommand(stockItem!), cancellationToken);
//        //    return new CreatedResult();
//        //}
//    }
//}
