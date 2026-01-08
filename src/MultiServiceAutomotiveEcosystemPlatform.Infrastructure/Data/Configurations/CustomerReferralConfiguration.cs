using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class CustomerReferralConfiguration : IEntityTypeConfiguration<CustomerReferral>
{
    public void Configure(EntityTypeBuilder<CustomerReferral> builder)
    {
        builder.ToTable("CustomerReferrals");

        builder.HasKey(r => r.CustomerReferralId);

        builder.Property(r => r.CustomerReferralId)
            .ValueGeneratedNever();

        builder.Property(r => r.TenantId)
            .IsRequired();

        builder.HasIndex(r => r.TenantId);

        builder.Property(r => r.ReferrerCustomerId)
            .IsRequired();

        builder.HasIndex(r => new { r.TenantId, r.ReferrerCustomerId });

        builder.Property(r => r.ReferrerCode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.RefereeCustomerId);

        builder.HasIndex(r => new { r.TenantId, r.RefereeCustomerId });

        builder.Property(r => r.RefereeEmail)
            .HasMaxLength(256);

        builder.Property(r => r.RefereePhone)
            .HasMaxLength(20);

        builder.Property(r => r.RefereeName)
            .HasMaxLength(200);

        builder.Property(r => r.TargetProfessionalId);

        builder.Property(r => r.TargetServiceType)
            .HasMaxLength(100);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.ConvertedAt);

        builder.Property(r => r.RewardStatus)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.RewardAmount)
            .HasPrecision(18, 2);

        builder.Property(r => r.RewardType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.RewardPaidAt);

        builder.Property(r => r.ReferralSource)
            .HasMaxLength(100);

        builder.Property(r => r.UtmCampaign)
            .HasMaxLength(100);

        builder.Property(r => r.UtmSource)
            .HasMaxLength(100);

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.Property(r => r.ExpiresAt);
    }
}
