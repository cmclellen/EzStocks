namespace EzStocks.Api.Infrastructure.Alphavantage
{
    public class AlphavantageSettings
    {
        public const string ConfigurationSection = "Alphavantage";

        public required string ApiBaseUrl { get; set; }
        public required string ApiKey { get; set; }
    }
}
