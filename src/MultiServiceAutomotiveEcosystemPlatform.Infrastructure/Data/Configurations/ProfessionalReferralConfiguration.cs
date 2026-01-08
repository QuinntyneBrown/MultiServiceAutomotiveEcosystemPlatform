using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class ProfessionalReferralConfiguration : IEntityTypeConfiguration<ProfessionalReferral>
{
    public void Configure(EntityTypeBuilder<ProfessionalReferral> builder)
    {
        builder.ToTable("ProfessionalReferrals");

        builder.HasKey(r => r.ProfessionalReferralId);

        builder.Property(r => r.ProfessionalReferralId)
            .ValueGeneratedNever();

        builder.Property(r => r.TenantId)
            .IsRequired();

        builder.HasIndex(r => r.TenantId);

        builder.Property(r => r.SourceProfessionalId)
            .IsRequired();

        builder.HasIndex(r => new { r.TenantId, r.SourceProfessionalId });

        builder.Property(r => r.TargetProfessionalId)
            .IsRequired();

        builder.HasIndex(r => new { r.TenantId, r.TargetProfessionalId });

        builder.Property(r => r.CustomerId)
            .IsRequired();

        builder.Property(r => r.Reason)
            .HasMaxLength(500);

        builder.Property(r => r.ServiceNeeded)
            .HasMaxLength(200);

        builder.Property(r => r.Notes)
            .HasColumnType("nvarchar(max)");

        builder.Property(r => r.Priority)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.AcceptedAt);

        builder.Property(r => r.CompletedAt);

        builder.Property(r => r.DeclinedReason)
            .HasMaxLength(500);

        builder.Property(r => r.DiscountOffered)
            .IsRequired();

        builder.Property(r => r.DiscountType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(r => r.DiscountValue)
            .HasPrecision(18, 2);

        builder.Property(r => r.DiscountCode)
            .HasMaxLength(50);

        builder.Property(r => r.DiscountUsed)
            .IsRequired();

        builder.Property(r => r.FollowUpRequired)
            .IsRequired();

        builder.Property(r => r.FollowUpDate);

        builder.Property(r => r.FollowUpNotes)
            .HasColumnType("nvarchar(max)");

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.UpdatedAt)
            .IsRequired();

        builder.Property(r => r.ExpiresAt);
    }
}
