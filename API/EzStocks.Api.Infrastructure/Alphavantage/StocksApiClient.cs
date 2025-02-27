using EzStocks.Api.Infrastructure.Alphavantage.Mappers;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json.Nodes;

namespace EzStocks.Api.Infrastructure.Alphavantage
{
    public class StocksApiClient(
        IOptions<AlphavantageSettings> alphavantageSettingsOptions, 
        IGetStockPriceResponseMapper getStockPriceResponseMapper) : Application.Services.IStocksApiClient
    {
        AlphavantageSettings AlphavantageSettings => alphavantageSettingsOptions.Value;

        public async Task<Application.Services.GetStockPriceResponse> GetStockPriceAsync(string symbol, CancellationToken cancellationToken)
        {
            // https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo
            var options = new RestClientOptions(AlphavantageSettings.ApiBaseUrl);
            var client = new RestClient(options);
            var request = new RestRequest("query");
            request.AddQueryParameter("function", "TIME_SERIES_DAILY");
            request.AddQueryParameter("symbol", symbol);
            request.AddQueryParameter("apikey", AlphavantageSettings.ApiKey);

            var response = await client.GetAsync(request, cancellationToken);

            return getStockPriceResponseMapper.MapFromJson(response.Content!);

        }
    }
}
