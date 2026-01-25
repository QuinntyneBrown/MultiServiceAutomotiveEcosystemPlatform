using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Referral.Core.Entities;

namespace Referral.Infrastructure.Data.Configurations;

public class ProfessionalReferralConfiguration : IEntityTypeConfiguration<ProfessionalReferral>
{
    public void Configure(EntityTypeBuilder<ProfessionalReferral> builder)
    {
        builder.ToTable("ProfessionalReferrals");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.TenantId).IsRequired();
        builder.Property(r => r.ReferringProfessionalId).IsRequired();
        builder.Property(r => r.ReferredBusinessName).IsRequired().HasMaxLength(200);
        builder.Property(r => r.ReferredEmail).IsRequired().HasMaxLength(256);
        builder.Property(r => r.ReferredPhone).HasMaxLength(20);
        builder.Property(r => r.ContactPersonName).HasMaxLength(100);
        builder.Property(r => r.ReferralSource).HasMaxLength(100);
        builder.Property(r => r.ReferralCode).HasMaxLength(50);
        builder.Property(r => r.Notes).HasMaxLength(2000);
        builder.Property(r => r.RewardAmount).HasPrecision(18, 2);
        builder.Property(r => r.RewardType).HasMaxLength(100);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(r => r.RewardStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(r => r.TenantId);
        builder.HasIndex(r => r.ReferringProfessionalId);
        builder.HasIndex(r => r.ReferredEmail);
        builder.HasIndex(r => r.Status);
    }
}
