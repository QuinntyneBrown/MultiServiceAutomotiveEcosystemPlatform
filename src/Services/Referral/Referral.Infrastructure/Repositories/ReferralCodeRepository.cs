using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;

namespace Referral.Infrastructure.Repositories;

public class ReferralCodeRepository : IReferralCodeRepository
{
    private readonly ReferralDbContext _context;

    public ReferralCodeRepository(ReferralDbContext context)
    {
        _context = context;
    }

    public async Task<ReferralCode?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes.FindAsync([id], cancellationToken);
    }

    public async Task<ReferralCode?> GetByCodeAsync(Guid tenantId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .FirstOrDefaultAsync(r => r.Code == code, cancellationToken);
    }

    public async Task<IReadOnlyList<ReferralCode>> GetByCustomerIdAsync(
        Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .Where(r => r.CustomerId == customerId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<ReferralCode>> GetByProfessionalIdAsync(
        Guid tenantId, Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes
            .Where(r => r.ProfessionalId == professionalId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(ReferralCode code, CancellationToken cancellationToken = default)
    {
        await _context.ReferralCodes.AddAsync(code, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(ReferralCode code)
    {
        _context.ReferralCodes.Update(code);
    }

    public async Task<bool> CodeExistsAsync(Guid tenantId, string code, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralCodes.AnyAsync(r => r.Code == code, cancellationToken);
    }
}
