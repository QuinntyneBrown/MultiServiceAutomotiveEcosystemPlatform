using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tenant.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Core.Entities.Tenant>
{
    public void Configure(EntityTypeBuilder<Core.Entities.Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Slug)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.DisplayName)
            .HasMaxLength(200);

        builder.Property(t => t.LogoUrl)
            .HasMaxLength(500);

        builder.Property(t => t.PrimaryColor)
            .HasMaxLength(50);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.HasIndex(t => t.Status);
    }
}
