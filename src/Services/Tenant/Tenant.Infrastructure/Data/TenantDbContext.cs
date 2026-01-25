using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Persistence;

namespace Tenant.Infrastructure.Data;

public class TenantDbContext : BaseDbContext
{
    public DbSet<Core.Entities.Tenant> Tenants => Set<Core.Entities.Tenant>();

    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);
    }
}
