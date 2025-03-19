using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Utils;
using EzStocks.Api.Infrastructure.PolygonIO.DTOs;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;

namespace EzStocks.Api.Infrastructure.PolygonIO
{
    public class PolygonIOStocksApiClient(
        IOptions<PolygonIOSettings> _polygonIOSettingsOptions) : IStocksApiClient
    {
        private PolygonIOSettings PolygonIOSettings => _polygonIOSettingsOptions.Value;

        private RestRequest CreateQueryRequest(string urlSuffix)
        {
            var restRequest = new RestRequest(urlSuffix);
            restRequest.AddHeader("Host", "api.polygon.io");
            restRequest.AddHeader("Authorization", $"Bearer {PolygonIOSettings.ApiKey}");
            return restRequest;
        }

        private RestClient GetClient()
        {
            var jsonSerializerOptions = new JsonSerializerOptions(JsonSerializerOptions.Web) {
                
            };
            var options = new RestClientOptions(PolygonIOSettings.ApiBaseUrl);
            var client = new RestClient(
                options,
                configureSerialization: s => s.UseSystemTextJson(jsonSerializerOptions));
            return client;
        }

        private DateTime GetEasternTime(DateTime dtUtc)
        {
            DateTime etTime = TimeZoneInfo.ConvertTimeFromUtc(dtUtc, ETTimeZoneInfo);
            return etTime;
        }

        private TimeZoneInfo ETTimeZoneInfo => TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

        private string ToDateString(DateTime dt)
        {
            return dt.ToString("yyyy-MM-dd");
        }

        public async Task<GetStockPriceResponse> GetStockPriceAsync(GetStockPriceRequest request, CancellationToken cancellationToken)
        {
            using var client = GetClient();

            var dt = GetEasternTime(DateTimeProvider.Current.UtcNow.AddDays(-3));
            var queryRequest = CreateQueryRequest("/v1/open-close/{stocksTicker}/{date}")
                .AddUrlSegment("stocksTicker", request.Symbol)
                .AddUrlSegment("date", ToDateString(dt));
            var ohlcvItem = await client.GetAsync<DTOs.OhlcvItemDto>(queryRequest, cancellationToken);
            if (ohlcvItem is null)
            {
                throw new Exception($"Failed to search for symbol");
            }

            var appOhlcvItem = new OhlcvItem
            {
                Date = DateOnly.FromDateTime(dt),
                Close = ohlcvItem.Close,
            };
            return new GetStockPriceResponse(ohlcvItem.Symbol, ETTimeZoneInfo, new List<OhlcvItem> { appOhlcvItem });
        }

        public Task<SearchForSymbolResponse> SearchForSymbolAsync(SearchForSymbolRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<GetAllTickersResponse> GetAllTickersAsync(GetAllTickersRequest request, CancellationToken cancellationToken)
        {
            using var client = GetClient();

            var queryRequest = CreateQueryRequest("/v3/reference/tickers")
                .AddQueryParameter("market", "stocks")
                .AddQueryParameter("active", "true")
                .AddQueryParameter("order", "asc")
                .AddQueryParameter("limit", "100")
                .AddQueryParameter("sort", "ticker");

            
            var v3ReferenceTickersResponseDto = await client.GetAsync<V3ReferenceTickersResponseDto>(queryRequest, cancellationToken);
            if(v3ReferenceTickersResponseDto is null)
            {
                throw new Exception($"Failed retrieving tickers.");
            }

            var items = v3ReferenceTickersResponseDto.Results.Select(i=>new TickerItem(i.Ticker)).ToList();

            return new GetAllTickersResponse(items);
        }
    }
}
