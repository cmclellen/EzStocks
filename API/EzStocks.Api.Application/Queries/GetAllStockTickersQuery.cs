using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetAllStockTickersQuery: IRequest<IList<Dtos.StockTicker>>;
}
