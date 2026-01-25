using Microsoft.EntityFrameworkCore;
using Customer.Core.Interfaces;

namespace Customer.Infrastructure.Data;

public class CustomerRepository : ICustomerRepository
{
    private readonly CustomerDbContext _context;

    public CustomerRepository(CustomerDbContext context)
    {
        _context = context;
    }

    public async Task<Core.Entities.Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Include(c => c.OwnershipHistory)
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Core.Entities.Customer?> GetByEmailAsync(Guid tenantId, string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Email == email.ToLowerInvariant(), cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.Customer>> GetByTenantAsync(Guid tenantId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .IgnoreQueryFilters()
            .Where(c => c.TenantId == tenantId)
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .IgnoreQueryFilters()
            .CountAsync(c => c.TenantId == tenantId, cancellationToken);
    }

    public async Task<IReadOnlyList<Core.Entities.Customer>> GetByOwnerAsync(Guid tenantId, Guid ownerId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .IgnoreQueryFilters()
            .Where(c => c.TenantId == tenantId && c.OwnerProfessionalId == ownerId)
            .OrderBy(c => c.LastName)
            .ThenBy(c => c.FirstName)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Core.Entities.Customer customer, CancellationToken cancellationToken = default)
    {
        await _context.Customers.AddAsync(customer, cancellationToken);
    }

    public void Update(Core.Entities.Customer customer)
    {
        _context.Customers.Update(customer);
    }

    public void Delete(Core.Entities.Customer customer)
    {
        _context.Customers.Remove(customer);
    }

    public async Task<bool> EmailExistsAsync(Guid tenantId, string email, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Customers
            .IgnoreQueryFilters()
            .Where(c => c.TenantId == tenantId && c.Email == email.ToLowerInvariant());

        if (excludeId.HasValue)
        {
            query = query.Where(c => c.Id != excludeId.Value);
        }

        return await query.AnyAsync(cancellationToken);
    }
}
