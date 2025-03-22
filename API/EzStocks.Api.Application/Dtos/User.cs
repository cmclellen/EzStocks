namespace EzStocks.Api.Application.Dtos
{
    public class User
    {
        public Guid UserId { get; set; }
        public required string FirstNames { get; set; }
        public required string LastName { get; set; }
        public required IList<StockTickerTiny> StockTickers { get; set; } = new List<StockTickerTiny>();
    }
}
