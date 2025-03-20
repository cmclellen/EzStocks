using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzStocks.Api.Persistence.EntityTypeConfigurations
{
    public class LookupEntityTypeConfiguration : IEntityTypeConfiguration<Lookup>
    {
        public void Configure(EntityTypeBuilder<Lookup> builder)
        {
            builder.ToContainer("Lookup");
            builder.HasKey(e => e.Id);
            builder.HasDiscriminator(e => e.Type)
                .HasValue<StockItem>(nameof(StockItem))
                .HasValue<StockPriceItem>(nameof(StockPriceItem))
                .HasValue<StockTicker>(nameof(StockTicker));
            builder.HasPartitionKey(e => e.Type);
        }
    }
}
