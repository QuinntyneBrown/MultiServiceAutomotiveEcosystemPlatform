using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Professional.Infrastructure.Data.Configurations;

public class TenantCacheConfiguration : IEntityTypeConfiguration<TenantCache>
{
    public void Configure(EntityTypeBuilder<TenantCache> builder)
    {
        builder.ToTable("TenantCache");

        builder.HasKey(t => t.TenantId);

        builder.Property(t => t.Name).IsRequired().HasMaxLength(200);
        builder.Property(t => t.Slug).IsRequired().HasMaxLength(200);
        builder.Property(t => t.LastUpdated).IsRequired();
    }
}
