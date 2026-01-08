using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class ReferralStatsConfiguration : IEntityTypeConfiguration<ReferralStats>
{
    public void Configure(EntityTypeBuilder<ReferralStats> builder)
    {
        builder.ToTable("ReferralStats");

        builder.HasKey(s => s.ReferralStatsId);

        builder.Property(s => s.ReferralStatsId)
            .ValueGeneratedNever();

        builder.Property(s => s.TenantId)
            .IsRequired();

        builder.HasIndex(s => s.TenantId);

        builder.Property(s => s.EntityType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(s => s.EntityId)
            .IsRequired();

        builder.HasIndex(s => new { s.TenantId, s.EntityType, s.EntityId })
            .IsUnique();

        builder.Property(s => s.TotalReferralsSent)
            .IsRequired();

        builder.Property(s => s.SuccessfulReferrals)
            .IsRequired();

        builder.Property(s => s.PendingReferrals)
            .IsRequired();

        builder.Property(s => s.TotalRewardsEarned)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.RewardsPending)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.ReferralsReceived)
            .IsRequired();

        builder.Property(s => s.ReferralsGiven)
            .IsRequired();

        builder.Property(s => s.ReferralConversionRate)
            .HasPrecision(5, 2)
            .IsRequired();

        builder.Property(s => s.AvgDiscountGiven)
            .HasPrecision(18, 2)
            .IsRequired();

        builder.Property(s => s.CalculatedAt)
            .IsRequired();
    }
}
