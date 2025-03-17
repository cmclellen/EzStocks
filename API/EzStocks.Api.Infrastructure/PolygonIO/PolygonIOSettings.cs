namespace EzStocks.Api.Infrastructure.PolygonIO
{
    public class PolygonIOSettings
    {
        public const string ConfigurationSection = "PolygonIO";

        public required string ApiKey { get; set; }
        public required string ApiBaseUrl { get; set; }
    }
}
