using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EzStocks.Api.Application.Queries
{
    public record GetUserQuery(Guid UserId) : IRequest<Dtos.User?>;

    public class GetUserQueryHandler(
        ILogger<GetUserQueryHandler> _logger,
        IMapper _mapper,
        IUserRepository _userRepository) : IRequestHandler<GetUserQuery, Dtos.User?>
    {
        public async Task<Dtos.User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            using var _ = _logger.BeginScope(new Dictionary<string, object> { ["UserId"] = request.UserId });

            _logger.LogDebug("Retrieving user...");
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null)
            {
                throw new Exception("User not found.");
            }
            _logger.LogInformation("Retrieved user");

            var userDto = _mapper.Map<Domain.Entities.User, Dtos.User>(user);
            return userDto;
        }
    }
}
