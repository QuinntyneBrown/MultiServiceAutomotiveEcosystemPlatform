using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class ProfessionalSpecialtyConfiguration : IEntityTypeConfiguration<ProfessionalSpecialty>
{
    public void Configure(EntityTypeBuilder<ProfessionalSpecialty> builder)
    {
        builder.ToTable("ProfessionalSpecialties");

        builder.HasKey(s => s.ProfessionalSpecialtyId);

        builder.Property(s => s.ProfessionalSpecialtyId)
            .ValueGeneratedNever();

        builder.Property(s => s.ProfessionalId)
            .IsRequired();

        builder.HasIndex(s => s.ProfessionalId);

        builder.Property(s => s.SpecialtyId);

        builder.Property(s => s.CustomName)
            .HasMaxLength(200);

        builder.Property(s => s.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.YearsExperience);

        builder.Property(s => s.CertificationName)
            .HasMaxLength(200);

        builder.Property(s => s.CertificationIssuer)
            .HasMaxLength(200);

        builder.Property(s => s.CertificationDate);

        builder.Property(s => s.CertificationExpiry);

        builder.Property(s => s.CertificationDocumentUrl)
            .HasMaxLength(500);

        builder.Property(s => s.Verified)
            .IsRequired();

        builder.Property(s => s.VerifiedAt);

        builder.Property(s => s.VerifiedBy);

        builder.Property(s => s.DisplayOrder)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();

        builder.Property(s => s.UpdatedAt)
            .IsRequired();

        builder.HasOne(s => s.SpecialtyCatalog)
            .WithMany()
            .HasForeignKey(s => s.SpecialtyId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
