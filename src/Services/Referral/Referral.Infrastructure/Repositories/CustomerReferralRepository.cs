using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;

namespace Referral.Infrastructure.Repositories;

public class CustomerReferralRepository : ICustomerReferralRepository
{
    private readonly ReferralDbContext _context;

    public CustomerReferralRepository(ReferralDbContext context)
    {
        _context = context;
    }

    public async Task<CustomerReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<CustomerReferral>> GetByReferringCustomerAsync(
        Guid tenantId, Guid customerId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .Where(r => r.ReferringCustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByReferringCustomerAsync(
        Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .Where(r => r.ReferringCustomerId == customerId)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(CustomerReferral referral, CancellationToken cancellationToken = default)
    {
        await _context.CustomerReferrals.AddAsync(referral, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(CustomerReferral referral)
    {
        _context.CustomerReferrals.Update(referral);
    }
}
