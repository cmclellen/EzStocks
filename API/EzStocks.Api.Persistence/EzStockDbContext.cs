using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EzStocks.Api.Persistence
{
    public class EzStockDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EzStockDbContext(DbContextOptions<EzStockDbContext> options):base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        //public DbSet<StockItem> StockItems { get; set; }
        public DbSet<StockPriceItem> StockPriceItems { get; set; }
        public DbSet<StockTicker> StockTickers { get; set; }
        public DbSet<StockHistoryItem> StockHistoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Persistence.AssemblyReference.Assembly);
        }
    }
}
