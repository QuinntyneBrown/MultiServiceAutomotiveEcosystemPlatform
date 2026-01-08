using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class CustomerOwnershipHistoryConfiguration : IEntityTypeConfiguration<CustomerOwnershipHistory>
{
    public void Configure(EntityTypeBuilder<CustomerOwnershipHistory> builder)
    {
        builder.ToTable("CustomerOwnershipHistories");

        builder.HasKey(h => h.CustomerOwnershipHistoryId);

        builder.Property(h => h.CustomerOwnershipHistoryId)
            .ValueGeneratedNever();

        builder.Property(h => h.TenantId)
            .IsRequired();

        builder.HasIndex(h => h.TenantId);

        builder.Property(h => h.CustomerId)
            .IsRequired();

        builder.HasIndex(h => new { h.TenantId, h.CustomerId });

        builder.Property(h => h.PreviousOwnerId);

        builder.Property(h => h.NewOwnerId)
            .IsRequired();

        builder.Property(h => h.Reason)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(h => h.TransferredBy)
            .IsRequired();

        builder.Property(h => h.TransferredAt)
            .IsRequired();
    }
}
