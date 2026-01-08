using Microsoft.EntityFrameworkCore;
using MultiServiceAutomotiveEcosystemPlatform.Core.Data;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate;
using MultiServiceAutomotiveEcosystemPlatform.Core.Models.CustomerAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Services;

public class CustomerService : ICustomerService
{
    private readonly IMultiServiceAutomotiveEcosystemPlatformContext _context;
    private readonly ITenantContext _tenantContext;

    public CustomerService(
        IMultiServiceAutomotiveEcosystemPlatformContext context,
        ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<Customer> CreateCustomerAsync(
        string email,
        string phone,
        string firstName,
        string lastName,
        Guid? ownerProfessionalId = null,
        string? source = null,
        CancellationToken cancellationToken = default)
    {
        if (await EmailExistsAsync(email, cancellationToken: cancellationToken))
            throw new InvalidOperationException($"A customer with email '{email}' already exists.");

        var customer = new Customer(
            _tenantContext.TenantId,
            email,
            phone,
            firstName,
            lastName,
            ownerProfessionalId,
            source);

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);

        return customer;
    }

    public async Task<Customer?> GetCustomerByIdAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.CustomerId == customerId && c.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<Customer?> GetCustomerByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();
        return await _context.Customers
            .FirstOrDefaultAsync(c => c.Email == normalizedEmail && c.TenantId == _tenantContext.TenantId, cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetCustomersByOwnerAsync(Guid ownerProfessionalId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Where(c => c.OwnerProfessionalId == ownerProfessionalId && c.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .Where(c => c.Status == CustomerStatus.Active && c.TenantId == _tenantContext.TenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer> UpdateCustomerAsync(
        Guid customerId,
        string email,
        string phone,
        string firstName,
        string lastName,
        CancellationToken cancellationToken = default)
    {
        var customer = await GetCustomerByIdAsync(customerId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer with ID '{customerId}' not found.");

        if (await EmailExistsAsync(email, customerId, cancellationToken))
            throw new InvalidOperationException($"A customer with email '{email}' already exists.");

        customer.UpdateContactInfo(email, phone);
        customer.UpdatePersonalInfo(firstName, lastName);

        await _context.SaveChangesAsync(cancellationToken);

        return customer;
    }

    public async Task TransferOwnershipAsync(
        Guid customerId,
        Guid newOwnerId,
        string reason,
        Guid transferredBy,
        CancellationToken cancellationToken = default)
    {
        var customer = await GetCustomerByIdAsync(customerId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer with ID '{customerId}' not found.");

        var previousOwnerId = customer.OwnerProfessionalId;

        var history = new CustomerOwnershipHistory(
            _tenantContext.TenantId,
            customerId,
            previousOwnerId,
            newOwnerId,
            reason,
            transferredBy);

        customer.TransferOwnership(newOwnerId);

        _context.CustomerOwnershipHistories.Add(history);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> AssignOwnerIfNotSetAsync(Guid customerId, Guid professionalId, CancellationToken cancellationToken = default)
    {
        var customer = await GetCustomerByIdAsync(customerId, cancellationToken);
        if (customer == null || customer.OwnerProfessionalId.HasValue)
            return false;

        customer.AssignOwner(professionalId);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task DeactivateCustomerAsync(Guid customerId, CancellationToken cancellationToken = default)
    {
        var customer = await GetCustomerByIdAsync(customerId, cancellationToken)
            ?? throw new InvalidOperationException($"Customer with ID '{customerId}' not found.");

        customer.Deactivate();
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> EmailExistsAsync(string email, Guid? excludeCustomerId = null, CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.ToLowerInvariant().Trim();

        var query = _context.Customers
            .Where(c => c.Email == normalizedEmail && c.TenantId == _tenantContext.TenantId);

        if (excludeCustomerId.HasValue)
            query = query.Where(c => c.CustomerId != excludeCustomerId.Value);

        return await query.AnyAsync(cancellationToken);
    }
}
