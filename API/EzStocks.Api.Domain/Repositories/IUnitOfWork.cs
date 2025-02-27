namespace EzStocks.Api.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken);
    }
}
