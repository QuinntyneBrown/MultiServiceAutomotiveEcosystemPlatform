using Microsoft.EntityFrameworkCore;
using Tenant.Core.Interfaces;

namespace Tenant.Infrastructure.Data;

public class TenantRepository : ITenantRepository
{
    private readonly TenantDbContext _context;

    public TenantRepository(TenantDbContext context)
    {
        _context = context;
    }

    public async Task<Core.Entities.Tenant?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants.FindAsync([id], cancellationToken);
    }

    public async Task<Core.Entities.Tenant?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .FirstOrDefaultAsync(t => t.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.Tenant>> GetAllAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Tenants
            .OrderBy(t => t.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Tenants.CountAsync(cancellationToken);
    }

    public async Task AddAsync(Core.Entities.Tenant tenant, CancellationToken cancellationToken = default)
    {
        await _context.Tenants.AddAsync(tenant, cancellationToken);
    }

    public void Update(Core.Entities.Tenant tenant)
    {
        _context.Tenants.Update(tenant);
    }

    public void Delete(Core.Entities.Tenant tenant)
    {
        _context.Tenants.Remove(tenant);
    }

    public async Task<bool> SlugExistsAsync(string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Tenants.Where(t => t.Slug == slug);

        if (excludeId.HasValue)
        {
            query = query.Where(t => t.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
