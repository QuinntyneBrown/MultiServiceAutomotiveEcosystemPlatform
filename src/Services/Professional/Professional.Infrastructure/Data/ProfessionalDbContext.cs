using Microsoft.EntityFrameworkCore;
using Professional.Core.Entities;
using Shared.Infrastructure.Persistence;
using Shared.Infrastructure.Services;

namespace Professional.Infrastructure.Data;

public class ProfessionalDbContext : BaseDbContext
{
    private readonly ITenantContext _tenantContext;

    public DbSet<Core.Entities.Professional> Professionals => Set<Core.Entities.Professional>();
    public DbSet<ProfessionalSpecialty> ProfessionalSpecialties => Set<ProfessionalSpecialty>();
    public DbSet<SpecialtyCatalog> SpecialtyCatalogs => Set<SpecialtyCatalog>();
    public DbSet<TenantCache> TenantCache => Set<TenantCache>();

    public ProfessionalDbContext(DbContextOptions<ProfessionalDbContext> options, ITenantContext tenantContext)
        : base(options, tenantContext)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProfessionalDbContext).Assembly);
    }

    protected override void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Professional>()
            .HasQueryFilter(p => p.TenantId == _tenantContext.TenantId);
    }
}
