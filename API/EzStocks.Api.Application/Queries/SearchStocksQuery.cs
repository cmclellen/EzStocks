using AutoMapper;
using EzStocks.Api.Application.Services;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record SearchStocksResponse(IList<Dtos.TickerSymbol> Items);

    public record SearchStocksQuery(string SearchText) : IRequest<SearchStocksResponse>;

    public class SearchStocksQueryHandler(
        IMapper _mapper,
        IStocksApiClient _stocksApiClient
        ) : IRequestHandler<SearchStocksQuery, SearchStocksResponse>
    {
        public async Task<SearchStocksResponse> Handle(SearchStocksQuery request, CancellationToken cancellationToken)
        {
            SearchForSymbolResponse searchForSymbolResponse = await _stocksApiClient.SearchForSymbolAsync(new SearchForSymbolRequest(request.SearchText), cancellationToken);
            var result = searchForSymbolResponse.Symbols.Select(_mapper.Map<TickerSymbol, Dtos.TickerSymbol>).ToList();
            return new SearchStocksResponse(result);
        }
    }
}
