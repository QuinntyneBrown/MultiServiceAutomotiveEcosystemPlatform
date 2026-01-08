using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Services;

namespace MultiServiceAutomotiveEcosystemPlatform.Infrastructure.Data;

public class MultiServiceAutomotiveEcosystemPlatformContext : DbContext, IMultiServiceAutomotiveEcosystemPlatformContext
{
    private readonly ITenantContext? _tenantContext;

    public MultiServiceAutomotiveEcosystemPlatformContext(
        DbContextOptions<MultiServiceAutomotiveEcosystemPlatformContext> options,
        ITenantContext? tenantContext = null)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    // Tenant Aggregate
    public DbSet<Tenant> Tenants => Set<Tenant>();

    // Customer Aggregate
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<CustomerOwnershipHistory> CustomerOwnershipHistories => Set<CustomerOwnershipHistory>();

    // Professional Aggregate
    public DbSet<Professional> Professionals => Set<Professional>();
    public DbSet<ProfessionalSpecialty> ProfessionalSpecialties => Set<ProfessionalSpecialty>();
    public DbSet<SpecialtyCatalog> SpecialtyCatalogs => Set<SpecialtyCatalog>();

    // Referral Aggregate
    public DbSet<CustomerReferral> CustomerReferrals => Set<CustomerReferral>();
    public DbSet<ProfessionalReferral> ProfessionalReferrals => Set<ProfessionalReferral>();
    public DbSet<ReferralCode> ReferralCodes => Set<ReferralCode>();
    public DbSet<ReferralStats> ReferralStats => Set<ReferralStats>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MultiServiceAutomotiveEcosystemPlatformContext).Assembly);

        // Apply global query filters for multi-tenancy
        ApplyGlobalFilters(modelBuilder);
    }

    private void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        // Apply TenantId filter to all aggregate roots
        modelBuilder.Entity<Customer>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<Professional>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<CustomerReferral>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ProfessionalReferral>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ReferralCode>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
        modelBuilder.Entity<ReferralStats>().HasQueryFilter(e => _tenantContext == null || e.TenantId == _tenantContext.TenantId);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return base.SaveChangesAsync(cancellationToken);
    }
}
