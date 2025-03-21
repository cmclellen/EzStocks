using EzStocks.Api.Application.Services;
using EzStocks.Api.Domain.Utils;
using EzStocks.Api.Infrastructure.PolygonIO.DTOs;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Serializers.Json;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        public async Task<SearchStockTickersResponse> SearchStockTickersAsync(SearchStockTickersRequest request, CancellationToken cancellationToken)
        {
            using var client = GetClient();

            var url = "/v3/reference/tickers";
            var queryRequest = CreateQueryRequest(url);
            if (request.Cursor is not null)
            {
                queryRequest = queryRequest
                    .AddQueryParameter("cursor", request.Cursor);
            }
            else
            {
                queryRequest = queryRequest
                    .AddQueryParameter("exchange", "XNAS")
                    .AddQueryParameter("type", "CS")
                    .AddQueryParameter("market", "stocks")
                    .AddQueryParameter("active", "true")
                    .AddQueryParameter("order", "asc")
                    .AddQueryParameter("limit", request.Limit)
                    .AddQueryParameter("sort", "ticker");
                if (request.SearchText is not null)
                {
                    queryRequest = queryRequest.AddQueryParameter("ticker.gte", request.SearchText);
                }
            }

            var v3ReferenceTickersResponseDto = await client.GetAsync<V3ReferenceTickersResponseDto>(queryRequest, cancellationToken);
            if (v3ReferenceTickersResponseDto is null)
            {
                throw new Exception($"Failed retrieving tickers.");
            }

            var items = v3ReferenceTickersResponseDto.Results.Select(i => new TickerSymbol(i.Ticker, i.Name, i.Locale, i.CurrencyName)).ToList();

            return new SearchStockTickersResponse(items, v3ReferenceTickersResponseDto.Count, GetCursor(v3ReferenceTickersResponseDto.NextUrl));
        }

        private string? GetCursor(string? nextUrl)
        {
            var regex = new Regex(@"cursor=(?<cursor>.+)$");
            regex.Match(nextUrl ?? string.Empty).Groups.TryGetValue("cursor", out var cursor);
            return cursor?.Value;
        }
    }
}
