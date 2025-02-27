using EzStocks.Api.Application.Services;
using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using Microsoft.Extensions.Options;
using RestSharp;

namespace EzStocks.Api.Infrastructure.Alphavantage
{
    public class StocksApiClient(
        IOptions<AlphavantageSettings> alphavantageSettingsOptions, 
        IGetStockPriceResponseMapper getStockPriceResponseMapper) : Application.Services.IStocksApiClient
    {
        AlphavantageSettings AlphavantageSettings => alphavantageSettingsOptions.Value;

        public async Task<Application.Services.GetStockPriceResponse> GetStockPriceAsync(GetStockPriceRequest request, CancellationToken cancellationToken)
        {
            // https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo
            var options = new RestClientOptions(AlphavantageSettings.ApiBaseUrl);
            var client = new RestClient(options);
            var restRequest = new RestRequest("query");
            restRequest.AddQueryParameter("function", "TIME_SERIES_DAILY");
            restRequest.AddQueryParameter("symbol", request.Symbol);
            restRequest.AddQueryParameter("apikey", AlphavantageSettings.ApiKey);

            var response = await client.GetAsync(restRequest, cancellationToken);

            return getStockPriceResponseMapper.MapFromJson(response.Content!);
        }
    }
}
