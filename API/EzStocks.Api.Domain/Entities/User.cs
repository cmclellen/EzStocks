using System.Diagnostics;

namespace EzStocks.Api.Domain.Entities
{
    [DebuggerDisplay("{LastName}, {FirstNames}")]
    public class User
    {
        public Guid? Id { get; set; }
        public Guid UserId { get; set; }
        public required string FirstNames { get; set; }
        public required string LastName { get; set; }
        public required List<UserStockTicker> StockTickers { get; set; } = new List<UserStockTicker>();
    }

    [DebuggerDisplay("{Ticker}")]
    public class UserStockTicker
    {
        public Guid? Id { get; set; }
        public required string Ticker { get; set; }
    }
}
