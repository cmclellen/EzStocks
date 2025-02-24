﻿using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace EzStocks.Api.Persistence
{
    public class EzStockDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public EzStockDbContext(DbContextOptions<EzStockDbContext> options):base(options)
        {
            
        }

        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<StockHistoryItem> StockHistoryItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Persistence.AssemblyReference.Assembly);
        }
    }
}
