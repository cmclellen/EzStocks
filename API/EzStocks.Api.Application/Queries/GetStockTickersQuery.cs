using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStockTickersQuery(IList<string>? Tickers = null): IRequest<IList<Dtos.StockTicker>>;

    public class GetStockTickersQueryHandler(
        IMapper mapper,
        IStockTickerRepository stockTickerRepository) : IRequestHandler<GetStockTickersQuery, IList<Dtos.StockTicker>>
    {
        public async Task<IList<Dtos.StockTicker>> Handle(GetStockTickersQuery request, CancellationToken cancellationToken)
        {
            var stockTickers = await stockTickerRepository.GetByTickersAsync(request.Tickers, cancellationToken);
            return stockTickers.Select(mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();
        }
    }
}
