using EzStocks.Api.Domain.Repositories;

namespace EzStocks.Api.Persistence.Repositories
{
    public class UnitOfWork(Persistence.EzStockDbContext ezStockDbContext) : IUnitOfWork
    {
        public async Task CommitAsync(CancellationToken cancellationToken) 
        {
            await ezStockDbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
