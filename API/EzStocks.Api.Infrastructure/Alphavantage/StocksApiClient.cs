using Microsoft.Extensions.Options;
using RestSharp;

namespace EzStocks.Api.Infrastructure.Alphavantage
{
    public class StocksApiClient(IOptions<AlphavantageSettings> alphavantageSettingsOptions) : Application.Services.IStocksApiClient
    {
        AlphavantageSettings AlphavantageSettings => alphavantageSettingsOptions.Value;

        public async Task GetStockPriceAsync(string symbol, CancellationToken cancellationToken)
        {
            // https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo
            var options = new RestClientOptions("https://www.alphavantage.co");
            var client = new RestClient(options);
            var request = new RestRequest("query");
            request.AddQueryParameter("function", "TIME_SERIES_DAILY");
            request.AddQueryParameter("symbol", symbol);
            request.AddQueryParameter("apikey", AlphavantageSettings.ApiKey);

            var response = await client.GetAsync(request, cancellationToken);

            var k = response.Content;

        }
    }
}
