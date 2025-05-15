namespace EzStocks.Api.Infrastructure.Alphavantage
{
    [Obsolete("This class is obsolete, use PolygonIOStocksApiClient instead")]
    public class AlphavantageSettings
    {
        public const string ConfigurationSection = "Alphavantage";

        public required string ApiBaseUrl { get; set; }
        public required string ApiKey { get; set; }
    }
}
