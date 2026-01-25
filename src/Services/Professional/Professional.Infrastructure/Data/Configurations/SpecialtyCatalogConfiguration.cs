using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Professional.Core.Entities;

namespace Professional.Infrastructure.Data.Configurations;

public class SpecialtyCatalogConfiguration : IEntityTypeConfiguration<SpecialtyCatalog>
{
    public void Configure(EntityTypeBuilder<SpecialtyCatalog> builder)
    {
        builder.ToTable("SpecialtyCatalogs");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.Description).HasMaxLength(1000);
        builder.Property(s => s.Category).HasMaxLength(100);

        builder.HasIndex(s => s.Name).IsUnique();
        builder.HasIndex(s => s.Category);
        builder.HasIndex(s => s.IsActive);
    }
}
