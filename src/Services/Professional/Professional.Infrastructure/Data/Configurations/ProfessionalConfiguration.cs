using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Professional.Infrastructure.Data.Configurations;

public class ProfessionalConfiguration : IEntityTypeConfiguration<Core.Entities.Professional>
{
    public void Configure(EntityTypeBuilder<Core.Entities.Professional> builder)
    {
        builder.ToTable("Professionals");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.TenantId).IsRequired();
        builder.Property(p => p.BusinessName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.BusinessDescription).HasMaxLength(2000);
        builder.Property(p => p.BusinessPhone).HasMaxLength(50);
        builder.Property(p => p.BusinessEmail).HasMaxLength(256);
        builder.Property(p => p.Website).HasMaxLength(500);
        builder.Property(p => p.Slug).IsRequired().HasMaxLength(200);
        builder.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.LastName).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
        builder.Property(p => p.Phone).HasMaxLength(50);
        builder.Property(p => p.StreetAddress).HasMaxLength(500);
        builder.Property(p => p.City).HasMaxLength(100);
        builder.Property(p => p.State).HasMaxLength(100);
        builder.Property(p => p.PostalCode).HasMaxLength(20);
        builder.Property(p => p.Country).HasMaxLength(100);
        builder.Property(p => p.ProfileImageUrl).HasMaxLength(500);
        builder.Property(p => p.CoverImageUrl).HasMaxLength(500);
        builder.Property(p => p.ReferralCommissionRate).HasPrecision(5, 2);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(p => new { p.TenantId, p.Slug }).IsUnique();
        builder.HasIndex(p => new { p.TenantId, p.Email }).IsUnique();
        builder.HasIndex(p => p.TenantId);
        builder.HasIndex(p => p.Status);
        builder.HasIndex(p => p.IsVerified);
        builder.HasIndex(p => p.IsFeatured);

        builder.HasMany(p => p.Specialties)
            .WithOne()
            .HasForeignKey(s => s.ProfessionalId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
