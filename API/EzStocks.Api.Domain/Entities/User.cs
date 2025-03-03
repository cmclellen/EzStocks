namespace EzStocks.Api.Domain.Entities
{
    public class User
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public required string FirstNames { get; set; }
        public required string LastName { get; set; }
        public required IList<UserStockItem> StockItems { get; set; }
    }

    public class UserStockItem
    {
        public Guid? Id { get; set; }
        public required string Symbol { get; set; }
    }
}
