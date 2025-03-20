
namespace EzStocks.Api.Domain.Repositories
{
    public interface IStockTickerRepository
    {
        Task UpsertAsync(IList<string> list, CancellationToken cancellationToken);
    }
}
