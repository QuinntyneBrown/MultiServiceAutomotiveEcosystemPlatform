using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Professional.Core.Entities;

namespace Professional.Infrastructure.Data.Configurations;

public class ProfessionalSpecialtyConfiguration : IEntityTypeConfiguration<ProfessionalSpecialty>
{
    public void Configure(EntityTypeBuilder<ProfessionalSpecialty> builder)
    {
        builder.ToTable("ProfessionalSpecialties");

        builder.HasKey(ps => ps.Id);

        builder.Property(ps => ps.ProfessionalId).IsRequired();
        builder.Property(ps => ps.SpecialtyId).IsRequired();
        builder.Property(ps => ps.SpecialtyName).IsRequired().HasMaxLength(200);
        builder.Property(ps => ps.Certification).HasMaxLength(500);
        builder.Property(ps => ps.YearsOfExperience).IsRequired();

        builder.HasIndex(ps => new { ps.ProfessionalId, ps.SpecialtyId }).IsUnique();
        builder.HasIndex(ps => ps.SpecialtyId);
    }
}
