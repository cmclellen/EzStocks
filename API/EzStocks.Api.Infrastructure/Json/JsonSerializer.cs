using EzStocks.Api.Application.Json;

namespace EzStocks.Api.Infrastructure.Json
{
    public class JsonSerializer : IJsonSerializer
    {
        private System.Text.Json.JsonSerializerOptions JsonOptions = new System.Text.Json.JsonSerializerOptions(System.Text.Json.JsonSerializerDefaults.Web);

        public T? Deserialize<T>(string json)
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
    }
}
