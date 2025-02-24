namespace EzStocks.Api.Domain.Repositories
{
    public interface IUnitOfWork
    {
        Task Commit(CancellationToken cancellationToken);
    }
}
