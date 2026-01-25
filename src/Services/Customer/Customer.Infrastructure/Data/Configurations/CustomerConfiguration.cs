using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customer.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Core.Entities.Customer>
{
    public void Configure(EntityTypeBuilder<Core.Entities.Customer> builder)
    {
        builder.ToTable("Customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.TenantId).IsRequired();
        builder.Property(c => c.Email).IsRequired().HasMaxLength(256);
        builder.Property(c => c.Phone).HasMaxLength(50);
        builder.Property(c => c.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.LastName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.MiddleName).HasMaxLength(100);
        builder.Property(c => c.StreetAddress).HasMaxLength(500);
        builder.Property(c => c.City).HasMaxLength(100);
        builder.Property(c => c.State).HasMaxLength(100);
        builder.Property(c => c.PostalCode).HasMaxLength(20);
        builder.Property(c => c.Country).HasMaxLength(100);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.PreferredContactMethod)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasIndex(c => new { c.TenantId, c.Email }).IsUnique();
        builder.HasIndex(c => c.TenantId);
        builder.HasIndex(c => c.OwnerProfessionalId);
        builder.HasIndex(c => c.Status);

        builder.HasMany(c => c.OwnershipHistory)
            .WithOne()
            .HasForeignKey(h => h.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
