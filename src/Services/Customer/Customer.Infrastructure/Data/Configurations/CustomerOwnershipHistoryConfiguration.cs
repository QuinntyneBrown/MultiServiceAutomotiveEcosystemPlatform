using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class CustomerOwnershipHistoryConfiguration : IEntityTypeConfiguration<Core.Entities.CustomerOwnershipHistory>
{
    public void Configure(EntityTypeBuilder<Core.Entities.CustomerOwnershipHistory> builder)
    {
        builder.ToTable("CustomerOwnershipHistories");

        builder.HasKey(h => h.Id);

        builder.Property(h => h.CustomerId).IsRequired();
        builder.Property(h => h.NewOwnerId).IsRequired();
        builder.Property(h => h.Reason).IsRequired().HasMaxLength(500);
        builder.Property(h => h.TransferredAt).IsRequired();

        builder.HasIndex(h => h.CustomerId);
        builder.HasIndex(h => h.NewOwnerId);
    }
}
