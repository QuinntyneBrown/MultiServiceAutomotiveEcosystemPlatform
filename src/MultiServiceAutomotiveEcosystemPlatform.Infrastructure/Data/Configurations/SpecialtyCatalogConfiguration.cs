using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data.Configurations;

public class SpecialtyCatalogConfiguration : IEntityTypeConfiguration<SpecialtyCatalog>
{
    public void Configure(EntityTypeBuilder<SpecialtyCatalog> builder)
    {
        builder.ToTable("SpecialtyCatalogs");

        builder.HasKey(s => s.SpecialtyCatalogId);

        builder.Property(s => s.SpecialtyCatalogId)
            .ValueGeneratedNever();

        builder.Property(s => s.TenantId);

        builder.HasIndex(s => s.TenantId);

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(s => new { s.TenantId, s.Slug })
            .IsUnique();

        builder.Property(s => s.Category)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasColumnType("nvarchar(max)");

        builder.Property(s => s.Icon)
            .HasMaxLength(100);

        builder.Property(s => s.IsActive)
            .IsRequired();

        builder.Property(s => s.CreatedAt)
            .IsRequired();
    }
}
