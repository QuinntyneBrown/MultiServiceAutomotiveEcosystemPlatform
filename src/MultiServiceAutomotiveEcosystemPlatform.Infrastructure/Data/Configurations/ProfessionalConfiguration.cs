using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Professional>
{
    public void Configure(EntityTypeBuilder<Professional> builder)
    {
        builder.ToTable("Professionals");

        builder.HasKey(p => p.ProfessionalId);

        builder.Property(p => p.ProfessionalId)
            .ValueGeneratedNever();

        builder.Property(p => p.TenantId)
            .IsRequired();

        builder.HasIndex(p => p.TenantId);

        builder.Property(p => p.UserId)
            .IsRequired();

        builder.Property(p => p.BusinessName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.BusinessType)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.LicenseNumber)
            .HasMaxLength(100);

        builder.Property(p => p.TaxId)
            .HasMaxLength(100);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Title)
            .HasMaxLength(100);

        builder.Property(p => p.Bio)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.PhoneBusiness)
            .HasMaxLength(20);

        builder.Property(p => p.Website)
            .HasMaxLength(500);

        builder.Property(p => p.AddressLine1)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(p => p.AddressLine2)
            .HasMaxLength(200);

        builder.Property(p => p.City)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.State)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.PostalCode)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(p => p.Country)
            .IsRequired()
            .HasMaxLength(2);

        builder.Property(p => p.Latitude)
            .HasPrecision(9, 6);

        builder.Property(p => p.Longitude)
            .HasPrecision(9, 6);

        builder.Property(p => p.ServiceRadiusMiles);

        builder.Property(p => p.ProfilePhotoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.CoverPhotoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.LogoUrl)
            .HasMaxLength(500);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(p => p.Verified)
            .IsRequired();

        builder.Property(p => p.Featured)
            .IsRequired();

        builder.Property(p => p.AcceptsReferrals)
            .IsRequired();

        builder.Property(p => p.AutoAcceptInquiries)
            .IsRequired();

        builder.Property(p => p.NotificationPreferences)
            .HasColumnType("nvarchar(max)");

        builder.Property(p => p.Slug)
            .IsRequired()
            .HasMaxLength(250);

        builder.HasIndex(p => new { p.TenantId, p.Slug })
            .IsUnique();

        builder.Property(p => p.CreatedAt)
            .IsRequired();

        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.Property(p => p.VerifiedAt);

        builder.HasMany(p => p.Specialties)
            .WithOne(s => s.Professional)
            .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
