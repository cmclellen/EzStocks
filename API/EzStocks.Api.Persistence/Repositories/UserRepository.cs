using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class UserRepository(EzStockDbContext ezStockDbContext) : IUserRepository
    {
        public async Task CreateAsync(User user, CancellationToken cancellationToken)
        {
            await ezStockDbContext.Users.AddAsync(user);
        }
    }
}
