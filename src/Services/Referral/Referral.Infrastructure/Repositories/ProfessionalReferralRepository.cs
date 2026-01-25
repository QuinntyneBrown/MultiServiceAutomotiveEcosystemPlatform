using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;

namespace Referral.Infrastructure.Repositories;

public class ProfessionalReferralRepository : IProfessionalReferralRepository
{
    private readonly ReferralDbContext _context;

    public ProfessionalReferralRepository(ReferralDbContext context)
    {
        _context = context;
    }

    public async Task<ProfessionalReferral?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals.FindAsync([id], cancellationToken);
    }

    public async Task<IReadOnlyList<ProfessionalReferral>> GetByReferringProfessionalAsync(
        Guid tenantId, Guid professionalId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .Where(r => r.ReferringProfessionalId == professionalId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetCountByReferringProfessionalAsync(
        Guid tenantId, Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ProfessionalReferrals
            .Where(r => r.ReferringProfessionalId == professionalId)
            .CountAsync(cancellationToken);
    }

    public async Task AddAsync(ProfessionalReferral referral, CancellationToken cancellationToken = default)
    {
        await _context.ProfessionalReferrals.AddAsync(referral, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(ProfessionalReferral referral)
    {
        _context.ProfessionalReferrals.Update(referral);
    }
}
