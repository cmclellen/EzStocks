namespace EzStocks.Api.Functions.Messages
{
    public record FetchStockPricesMessage(IList<string> Symbols);
}
