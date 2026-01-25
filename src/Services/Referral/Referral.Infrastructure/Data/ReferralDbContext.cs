using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Shared.Infrastructure.Persistence;
using Shared.Infrastructure.Services;

namespace Referral.Infrastructure.Data;

public class ReferralDbContext : BaseDbContext
{
    private readonly ITenantContext _tenantContext;

    public DbSet<CustomerReferral> CustomerReferrals => Set<CustomerReferral>();
    public DbSet<ProfessionalReferral> ProfessionalReferrals => Set<ProfessionalReferral>();
    public DbSet<ReferralCode> ReferralCodes => Set<ReferralCode>();
    public DbSet<ReferralStats> ReferralStats => Set<ReferralStats>();
    public DbSet<CustomerCache> CustomerCache => Set<CustomerCache>();
    public DbSet<ProfessionalCache> ProfessionalCache => Set<ProfessionalCache>();

    public ReferralDbContext(DbContextOptions<ReferralDbContext> options, ITenantContext tenantContext)
        : base(options, tenantContext)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReferralDbContext).Assembly);
    }

    protected override void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CustomerReferral>()
            .HasQueryFilter(r => r.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<ProfessionalReferral>()
            .HasQueryFilter(r => r.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<ReferralCode>()
            .HasQueryFilter(r => r.TenantId == _tenantContext.TenantId);

        modelBuilder.Entity<ReferralStats>()
            .HasQueryFilter(r => r.TenantId == _tenantContext.TenantId);
    }
}
