using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Services;

namespace Shared.Infrastructure.Persistence;

public abstract class BaseDbContext : DbContext
{
    private readonly ITenantContext? _tenantContext;

    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected BaseDbContext(DbContextOptions options) : base(options)
    {
    }

    protected BaseDbContext(DbContextOptions options, ITenantContext tenantContext) : base(options)
    {
        _tenantContext = tenantContext;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Type).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Content).IsRequired();
            entity.HasIndex(e => e.ProcessedAt);
            entity.HasIndex(e => new { e.ProcessedAt, e.CreatedAt });
        });

        ApplyGlobalFilters(modelBuilder);
    }

    protected virtual void ApplyGlobalFilters(ModelBuilder modelBuilder)
    {
        // Override in derived classes to apply tenant filters
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Added or EntityState.Modified);

        foreach (var entry in entries)
        {
            var now = DateTime.UtcNow;

            if (entry.State == EntityState.Added)
            {
                if (entry.Entity.GetType().GetProperty("CreatedAt") is { } createdAtProp)
                {
                    createdAtProp.SetValue(entry.Entity, now);
                }
            }

            if (entry.Entity.GetType().GetProperty("UpdatedAt") is { } updatedAtProp)
            {
                updatedAtProp.SetValue(entry.Entity, now);
            }
        }
    }

    protected Guid GetCurrentTenantId()
    {
        if (_tenantContext == null || !_tenantContext.HasTenant)
        {
            throw new InvalidOperationException("Tenant context is not available");
        }
        return _tenantContext.TenantId;
    }
}
