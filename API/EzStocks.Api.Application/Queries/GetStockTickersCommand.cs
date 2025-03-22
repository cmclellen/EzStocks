using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStockTickersCommand(IList<string>? tickers = null) : IRequest<IList<Dtos.StockTicker>>;

    public class GetStockTickersCommandHandler(
        IMapper mapper,
        IStockTickerRepository _stockTickerRepository) : IRequestHandler<GetStockTickersCommand, IList<Dtos.StockTicker>>
    {
        public async Task<IList<Dtos.StockTicker>> Handle(GetStockTickersCommand request, CancellationToken cancellationToken)
        {
            var stockTickers = await _stockTickerRepository.GetByTickersAsync(request.tickers, cancellationToken);
            IList<Dtos.StockTicker> result = stockTickers.Select(mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();
            return result;
        }
    }
}
