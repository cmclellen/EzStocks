using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzStocks.Api.Persistence.EntityTypeConfigurations
{
    public class StockHistoryItemEntityTypeConfiguration : IEntityTypeConfiguration<StockHistoryItem>
    {
        public void Configure(EntityTypeBuilder<StockHistoryItem> builder)
        {
            builder.ToContainer("StockHistoryItem");
        }
    }
}
