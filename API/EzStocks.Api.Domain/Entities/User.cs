namespace EzStocks.Api.Domain.Entities
{
    public class User
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public required string FirstNames { get; set; }
        public required string LastName { get; set; }
        public required List<UserStockTicker> StockItems { get; set; } = new List<UserStockTicker>();
    }

    public class UserStockTicker
    {
        public Guid? Id { get; set; }
        public required string Symbol { get; set; }
    }
}
