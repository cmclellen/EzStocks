using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetUserQuery(Guid UserId) : IRequest<Dtos.User?>;

    public class GetUserQueryHandler(
        IMapper mapper,
        IUserRepository _userRepository) : IRequestHandler<GetUserQuery, Dtos.User?>
    {
        public async Task<Dtos.User?> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);
            if(user is null)
            {
                throw new Exception("User not found.");
            }
            var userDto = mapper.Map<Domain.Entities.User, Dtos.User>(user);
            return userDto;
        }
    }
}
