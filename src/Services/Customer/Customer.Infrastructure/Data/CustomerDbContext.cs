using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;
using Shared.Infrastructure.Services;

namespace Customer.Infrastructure.Data;

public class CustomerDbContext : BaseDbContext
{
    private readonly ITenantContext _tenantContext;

    public DbSet<Core.Entities.Customer> Customers => Set<Core.Entities.Customer>();
    public DbSet<Core.Entities.CustomerOwnershipHistory> CustomerOwnershipHistories => Set<Core.Entities.CustomerOwnershipHistory>();
    public DbSet<TenantCache> TenantCache => Set<TenantCache>();

    public CustomerDbContext(DbContextOptions<CustomerDbContext> options, ITenantContext tenantContext)
        : base(options, tenantContext)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CustomerDbContext).Assembly);
    }

    protected override void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Core.Entities.Customer>()
            .HasQueryFilter(c => c.TenantId == _tenantContext.TenantId);
    }
}
