using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Referral.Infrastructure.Data.Configurations;

public class CustomerCacheConfiguration : IEntityTypeConfiguration<CustomerCache>
{
    public void Configure(EntityTypeBuilder<CustomerCache> builder)
    {
        builder.ToTable("CustomerCache");
        builder.HasKey(c => c.CustomerId);
        builder.Property(c => c.Email).IsRequired().HasMaxLength(256);
        builder.Property(c => c.FullName).IsRequired().HasMaxLength(200);
        builder.HasIndex(c => c.TenantId);
    }
}

public class ProfessionalCacheConfiguration : IEntityTypeConfiguration<ProfessionalCache>
{
    public void Configure(EntityTypeBuilder<ProfessionalCache> builder)
    {
        builder.ToTable("ProfessionalCache");
        builder.HasKey(p => p.ProfessionalId);
        builder.Property(p => p.BusinessName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(256);
        builder.HasIndex(p => p.TenantId);
    }
}
