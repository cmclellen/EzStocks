using EzStocks.Api.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Functions.Functions
{
    public class UserFunctions(
        ILogger<UserFunctions> _logger,
        ISender _sender)
    {
        [Function(nameof(CreateUser))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public async Task<IActionResult> CreateUser([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "users")] HttpRequest req, CancellationToken cancellationToken)
        {
            var user = await req.ReadFromJsonAsync<EzStocks.Api.Application.Dtos.User>();
            await _sender.Send(new Application.Commands.CreateUserCommand(user!), cancellationToken);
            return new CreatedResult();
        }
    }
}
