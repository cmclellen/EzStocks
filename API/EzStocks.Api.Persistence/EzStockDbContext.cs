using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence
{
    public class EzStockDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EzStockDbContext(DbContextOptions<EzStockDbContext> options):base(options)
        {
            
        }

        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<StockHistoryItem> StockHistoryItems { get; set; }
    }
}
