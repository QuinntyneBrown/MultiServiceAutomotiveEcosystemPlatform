using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Referral.Core.Interfaces;

namespace Referral.Infrastructure.Data;

public class CustomerReferralRepository : ICustomerReferralRepository
{
    private readonly ReferralDbContext _context;

    public CustomerReferralRepository(ReferralDbContext context) => _context = context;

    public async Task<CustomerReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<CustomerReferral>> GetByReferringCustomerAsync(
        Guid tenantId, Guid customerId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .IgnoreQueryFilters()
            .Where(r => r.TenantId == tenantId && r.ReferringCustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByReferringCustomerAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.CustomerReferrals
            .IgnoreQueryFilters()
            .CountAsync(r => r.TenantId == tenantId && r.ReferringCustomerId == customerId, cancellationToken);
    }

    public async Task AddAsync(CustomerReferral referral, CancellationToken cancellationToken = default)
    {
        await _context.CustomerReferrals.AddAsync(referral, cancellationToken);
    }

    public void Update(CustomerReferral referral)
    {
        _context.CustomerReferrals.Update(referral);
    }
}

public class ProfessionalReferralRepository : IProfessionalReferralRepository
{
    private readonly ReferralDbContext _context;

    public ProfessionalReferralRepository(ReferralDbContext context) => _context = context;

    public async Task<ProfessionalReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalReferral>> GetByReferringProfessionalAsync(
        Guid tenantId, Guid professionalId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .IgnoreQueryFilters()
            .Where(r => r.TenantId == tenantId && r.ReferringProfessionalId == professionalId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalReferral>> GetByReferredProfessionalAsync(
        Guid tenantId, Guid professionalId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .IgnoreQueryFilters()
            .Where(r => r.TenantId == tenantId && r.ReferredProfessionalId == professionalId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByReferringProfessionalAsync(Guid tenantId, Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .IgnoreQueryFilters()
            .CountAsync(r => r.TenantId == tenantId && r.ReferringProfessionalId == professionalId, cancellationToken);
    }

    public async Task AddAsync(ProfessionalReferral referral, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalReferrals.AddAsync(referral, cancellationToken);
    }

    public void Update(ProfessionalReferral referral)
    {
        _context.ProfessionalReferrals.Update(referral);
    }
}

public class ReferralCodeRepository : IReferralCodeRepository
{
    private readonly ReferralDbContext _context;

    public ReferralCodeRepository(ReferralDbContext context) => _context = context;

    public async Task<ReferralCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes.FindAsync([id], cancellationToken);
    }

    public async Task<ReferralCode?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Code == code.ToUpperInvariant(), cancellationToken);
    }

    public async Task<IReadOnlyList<ReferralCode>> GetByOwnerAsync(Guid tenantId, Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .IgnoreQueryFilters()
            .Where(c => c.TenantId == tenantId && c.OwnerId == ownerId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ReferralCode code, CancellationToken cancellationToken = default)
    {
        await _context.ReferralCodes.AddAsync(code, cancellationToken);
    }

    public void Update(ReferralCode code)
    {
        _context.ReferralCodes.Update(code);
    }

    public async Task<bool> CodeExistsAsync(Guid tenantId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .IgnoreQueryFilters()
            .AnyAsync(c => c.TenantId == tenantId && c.Code == code.ToUpperInvariant(), cancellationToken);
    }
}

public class ReferralStatsRepository : IReferralStatsRepository
{
    private readonly ReferralDbContext _context;

    public ReferralStatsRepository(ReferralDbContext context) => _context = context;

    public async Task<ReferralStats?> GetByOwnerAsync(Guid tenantId, Guid ownerId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralStats
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.TenantId == tenantId && s.OwnerId == ownerId, cancellationToken);
    }

    public async Task AddAsync(ReferralStats stats, CancellationToken cancellationToken = default)
    {
        await _context.ReferralStats.AddAsync(stats, cancellationToken);
    }

    public void Update(ReferralStats stats)
    {
        _context.ReferralStats.Update(stats);
    }
}
