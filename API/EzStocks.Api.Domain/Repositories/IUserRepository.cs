using EzStocks.Api.Domain.Entities;

namespace EzStocks.Api.Domain.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user, CancellationToken cancellationToken);
    }
}
