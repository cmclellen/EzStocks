using AutoMapper;
using EzStocks.Api.Domain.Repositories;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record GetStockTickersCommand(IList<string>? tickers = null) : IRequest<GetStockTickersResponse>;

    public record GetStockTickersResponse(IList<Dtos.StockTicker> StockTickers);

    public class GetStockTickersCommandHandler(
        IMapper mapper,
        IStockTickerRepository _stockTickerRepository) : IRequestHandler<GetStockTickersCommand, GetStockTickersResponse>
    {
        public async Task<GetStockTickersResponse> Handle(GetStockTickersCommand request, CancellationToken cancellationToken)
        {
            var stockTickers = await _stockTickerRepository.GetByTickersAsync(request.tickers, cancellationToken);
            IList<Dtos.StockTicker> result = stockTickers.Select(mapper.Map<Domain.Entities.StockTicker, Dtos.StockTicker>).ToList();
            return new GetStockTickersResponse(result);
        }
    }
}
