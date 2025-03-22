using EzStocks.Api.Domain.Entities;
using EzStocks.Api.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

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
            return await ezStockDbContext.Users//.Include(e => e.StockTickers)
                .FirstOrDefaultAsync(e=>e.UserId == userId);
        }
    }
}
