using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class ReferralCodeConfiguration : IEntityTypeConfiguration<ReferralCode>
{
    public void Configure(EntityTypeBuilder<ReferralCode> builder)
    {
        builder.ToTable("ReferralCodes");

        builder.HasKey(r => r.ReferralCodeId);

        builder.Property(r => r.ReferralCodeId)
            .ValueGeneratedNever();

        builder.Property(r => r.TenantId)
            .IsRequired();

        builder.HasIndex(r => r.TenantId);

        builder.Property(r => r.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(r => new { r.TenantId, r.Code })
            .IsUnique();

        builder.Property(r => r.CodeType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.CustomerId);

        builder.HasIndex(r => new { r.TenantId, r.CustomerId });

        builder.Property(r => r.ProfessionalId);

        builder.HasIndex(r => new { r.TenantId, r.ProfessionalId });

        builder.Property(r => r.CampaignId);

        builder.Property(r => r.MaxUses);

        builder.Property(r => r.CurrentUses)
            .IsRequired();

        builder.Property(r => r.RewardAmount)
            .HasPrecision(18, 2);

        builder.Property(r => r.DiscountPercentage)
            .HasPrecision(5, 2);

        builder.Property(r => r.IsActive)
            .IsRequired();

        builder.Property(r => r.ExpiresAt);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();
    }
}
