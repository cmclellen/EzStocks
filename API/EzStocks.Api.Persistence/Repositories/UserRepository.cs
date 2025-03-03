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

        public async Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await ezStockDbContext.Users.FindAsync(userId, cancellationToken);
        }
    }
}
