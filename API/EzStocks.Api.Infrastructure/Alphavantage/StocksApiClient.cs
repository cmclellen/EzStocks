using EzStocks.Api.Application.Services;
using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;

namespace EzStocks.Api.Infrastructure.Alphavantage
{
    public class StocksApiClient(
        ILogger<StocksApiClient> _logger,
        IOptions<AlphavantageSettings> _alphavantageSettingsOptions, 
        IGetStockPriceResponseMapper _getStockPriceResponseMapper,
        ISearchForSymbolResponseMapper _searchForSymbolResponseMapper) : IStocksApiClient
    {
        AlphavantageSettings AlphavantageSettings => _alphavantageSettingsOptions.Value;

        private RestRequest CreateQueryRequest(string functionName)
        {
            var restRequest = new RestRequest("query");
            restRequest.AddQueryParameter("function", functionName);
            restRequest.AddQueryParameter("apikey", AlphavantageSettings.ApiKey);
            return restRequest;
        }

        public async Task<GetStockPriceResponse> GetStockPriceAsync(GetStockPriceRequest request, CancellationToken cancellationToken)
        {
            const string FUNCTION = "TIME_SERIES_DAILY";
            using var _ = _logger.BeginScope(new Dictionary<string, object> { ["Symbol"] = request.Symbol, ["Function"] = FUNCTION });

            var options = new RestClientOptions(AlphavantageSettings.ApiBaseUrl);
            var client = new RestClient(options);
            var restRequest = CreateQueryRequest(FUNCTION);
            restRequest.AddQueryParameter("symbol", request.Symbol);

            var response = await client.GetAsync(restRequest, cancellationToken);
            var json = response.Content!;
            _logger.LogInformation("Response receieved [{JsonResponse}]", json);

            return _getStockPriceResponseMapper.MapFromJson(json);
        }

        public async Task<SearchForSymbolResponse> SearchForSymbolAsync(SearchForSymbolRequest request, CancellationToken cancellationToken)
        {
            const string FUNCTION = "SYMBOL_SEARCH";
            using var _ = _logger.BeginScope(new Dictionary<string, object> { ["SearchText"] = request.SearchText, ["Function"] = FUNCTION });

            var options = new RestClientOptions(AlphavantageSettings.ApiBaseUrl);
            var client = new RestClient(options);
            var restRequest = CreateQueryRequest(FUNCTION);
            restRequest.AddQueryParameter("keywords", request.SearchText);

            var response = await client.GetAsync(restRequest, cancellationToken);
            var json = response.Content!;
            _logger.LogInformation("Response receieved [{JsonResponse}]", json);

            return _searchForSymbolResponseMapper.MapFromJson(json);
        }
    }
}
