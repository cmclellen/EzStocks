using AutoMapper;
using EzStocks.Api.Application.Services;
using MediatR;

namespace EzStocks.Api.Application.Queries
{
    public record SearchStockTickersResponse(IList<Dtos.StockTickerSmall> StockTickers, int Count);

    public record SearchStockTickersQuery(string SearchText) : IRequest<SearchStockTickersResponse>;

    public class SearchStockTickersQueryHandler(
        IMapper _mapper,
        IStocksApiClient _stocksApiClient
        ) : IRequestHandler<SearchStockTickersQuery, SearchStockTickersResponse>
    {
        public async Task<SearchStockTickersResponse> Handle(SearchStockTickersQuery request, CancellationToken cancellationToken)
        {
            Services.SearchStockTickersResponse searchForSymbolResponse = await _stocksApiClient.SearchStockTickersAsync(new SearchStockTickersRequest(request.SearchText), cancellationToken);
            var response = _mapper.Map<Services.SearchStockTickersResponse, SearchStockTickersResponse>(searchForSymbolResponse);
            return response;
        }
    }
}
