using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Referral.Core.Entities;

namespace Referral.Infrastructure.Data.Configurations;

public class ReferralCodeConfiguration : IEntityTypeConfiguration<ReferralCode>
{
    public void Configure(EntityTypeBuilder<ReferralCode> builder)
    {
        builder.ToTable("ReferralCodes");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.Code).IsRequired().HasMaxLength(50);
        builder.Property(c => c.CampaignName).HasMaxLength(100);

        builder.HasIndex(c => new { c.TenantId, c.Code }).IsUnique();
        builder.HasIndex(c => c.CustomerId);
        builder.HasIndex(c => c.ProfessionalId);
        builder.HasIndex(c => c.IsActive);
    }
}
