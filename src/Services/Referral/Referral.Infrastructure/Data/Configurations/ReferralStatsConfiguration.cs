using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Referral.Core.Entities;

namespace Referral.Infrastructure.Data.Configurations;

public class ReferralStatsConfiguration : IEntityTypeConfiguration<ReferralStats>
{
    public void Configure(EntityTypeBuilder<ReferralStats> builder)
    {
        builder.ToTable("ReferralStats");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.TenantId).IsRequired();
        builder.Property(s => s.TotalRewardsEarned).HasPrecision(18, 2);

        builder.HasIndex(s => new { s.TenantId, s.CustomerId })
            .HasFilter("[CustomerId] IS NOT NULL");
        builder.HasIndex(s => new { s.TenantId, s.ProfessionalId })
            .HasFilter("[ProfessionalId] IS NOT NULL");
    }
}
