using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzStocks.Api.Persistence.EntityTypeConfigurations
{
    public class StockTickerEntityTypeConfiguration : IEntityTypeConfiguration<StockTicker>
    {
        public void Configure(EntityTypeBuilder<StockTicker> builder)
        {
            builder.ToContainer("StockTicker");
        }
    }
}
