using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzStocks.Api.Persistence.EntityTypeConfigurations
{
    public class StockItemEntityTypeConfiguration : IEntityTypeConfiguration<StockItem>
    {
        public void Configure(EntityTypeBuilder<StockItem> builder)
        {
            builder.ToContainer("StockItem");
            builder.HasKey(e => e.Id);
            builder.HasDiscriminator(e => e.Type).HasValue(nameof(StockItem));
            builder.HasPartitionKey(e => e.Type);
        }
    }
}
