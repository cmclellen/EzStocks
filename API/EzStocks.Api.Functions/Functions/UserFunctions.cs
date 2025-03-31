using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;

namespace EzStocks.Api.Functions.Functions
{
    public class UserFunctions(        
        ISender _sender)
    {
        [Function(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, CancellationToken cancellationToken)
        {
            var user = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.User>();
            await _sender.Send(new Application.Commands.CreateUserCommand(user!), cancellationToken);
            return new CreatedResult();
        }

        [Function(nameof(GetUser))]
        public async Task<IActionResult> GetUser([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "users/{userId:guid}")] HttpRequest req, Guid userId, CancellationToken cancellationToken)
        {
            var user = await _sender.Send(new Application.Queries.GetUserQuery(userId), cancellationToken);
            return new OkObjectResult(user);
        }

        [Function(nameof(AddUserStockTicker))]
        public async Task<IActionResult> AddUserStockTicker([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users/{userId:guid}/stock-tickers")] HttpRequest req, string ticker, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new Application.Commands.AddUserStockTickerCommand(ticker), cancellationToken);
            if (result.IsNotFound()) { 
                return new NotFoundResult();
            }
            return new OkResult();
        }
    }
}
