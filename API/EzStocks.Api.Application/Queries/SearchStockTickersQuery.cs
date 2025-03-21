using AutoMapper;
using EzStocks.Api.Application.Services;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record SearchStockTickersResponse(IList<Dtos.TickerSymbol> Items);

    public record SearchStockTickersQuery(string SearchText) : IRequest<SearchStockTickersResponse>;

    public class SearchStockTickersQueryHandler(
        IMapper _mapper,
        IStocksApiClient _stocksApiClient
        ) : IRequestHandler<SearchStockTickersQuery, SearchStockTickersResponse>
    {
        public async Task<SearchStockTickersResponse> Handle(SearchStockTickersQuery request, CancellationToken cancellationToken)
        {
            Services.SearchStockTickersResponse searchForSymbolResponse = await _stocksApiClient.SearchStockTickersAsync(new SearchStockTickersRequest(request.SearchText), cancellationToken);
            var result = searchForSymbolResponse.TickerSymbols.Select(_mapper.Map<TickerSymbol, Dtos.TickerSymbol>).ToList();
            return new SearchStockTickersResponse(result);
        }
    }
}
