using Microsoft.EntityFrameworkCore;
using Professional.Core.Interfaces;

namespace Professional.Infrastructure.Data;

public class ProfessionalRepository : IProfessionalRepository
{
    private readonly ProfessionalDbContext _context;

    public ProfessionalRepository(ProfessionalDbContext context)
    {
        _context = context;
    }

    public async Task<Core.Entities.Professional?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<Core.Entities.Professional?> GetBySlugAsync(Guid tenantId, string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .IgnoreQueryFilters()
            .Include(p => p.Specialties)
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Slug == slug, cancellationToken);
    }

    public async Task<Core.Entities.Professional?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.Professional>> GetByTenantAsync(Guid tenantId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .IgnoreQueryFilters()
            .Where(p => p.TenantId == tenantId)
            .Include(p => p.Specialties)
            .OrderBy(p => p.BusinessName)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Professionals
            .IgnoreQueryFilters()
            .CountAsync(p => p.TenantId == tenantId, cancellationToken);
    }

    public async Task AddAsync(Core.Entities.Professional professional, CancellationToken cancellationToken = default)
    {
        await _context.Professionals.AddAsync(professional, cancellationToken);
    }

    public void Update(Core.Entities.Professional professional)
    {
        _context.Professionals.Update(professional);
    }

    public void Delete(Core.Entities.Professional professional)
    {
        _context.Professionals.Remove(professional);
    }

    public async Task<bool> SlugExistsAsync(Guid tenantId, string slug, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Professionals
            .IgnoreQueryFilters()
            .Where(p => p.TenantId == tenantId && p.Slug == slug);

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Professionals
            .IgnoreQueryFilters()
            .Where(p => p.TenantId == tenantId && p.Email == email.ToLowerInvariant());

        if (excludeId.HasValue)
        {
            query = query.Where(p => p.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}

public class SpecialtyCatalogRepository : ISpecialtyCatalogRepository
{
    private readonly ProfessionalDbContext _context;

    public SpecialtyCatalogRepository(ProfessionalDbContext context)
    {
        _context = context;
    }

    public async Task<Core.Entities.SpecialtyCatalog?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.SpecialtyCatalogs.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.SpecialtyCatalog>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SpecialtyCatalogs
            .OrderBy(s => s.Category)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.SpecialtyCatalog>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SpecialtyCatalogs
            .Where(s => s.IsActive)
            .OrderBy(s => s.Category)
            .ThenBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Core.Entities.SpecialtyCatalog specialty, CancellationToken cancellationToken = default)
    {
        await _context.SpecialtyCatalogs.AddAsync(specialty, cancellationToken);
    }

    public void Update(Core.Entities.SpecialtyCatalog specialty)
    {
        _context.SpecialtyCatalogs.Update(specialty);
    }
}
