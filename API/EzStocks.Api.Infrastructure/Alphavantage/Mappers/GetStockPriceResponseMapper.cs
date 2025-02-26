using EzStocks.Api.Application.Services;
using System.Text.Json.Nodes;

namespace EzStocks.Api.Infrastructure.Alphavantage.Mappers
{
    public interface IGetStockPriceResponseMapper
    {
        GetStockPriceResponse MapFromText(string json);
    }
    public class GetStockPriceResponseMapper : IGetStockPriceResponseMapper
    {
        public GetStockPriceResponse MapFromText(string json)
        {
            var jsonObject = JsonObject.Parse(json);



            return new Application.Services.GetStockPriceResponse();
        }
    }
}
