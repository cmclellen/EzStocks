using EzStocks.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EzStocks.Api.Persistence.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToContainer("User");
            builder.HasKey(e => e.Id);
            builder.HasPartitionKey(e => e.UserId);
            builder.HasNoDiscriminator();
            builder.OwnsMany(e => e.StockItems);
        }
    }
}
