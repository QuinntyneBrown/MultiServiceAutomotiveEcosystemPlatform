using Microsoft.EntityFrameworkCore;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;

namespace Referral.Infrastructure.Repositories;

public class ReferralStatsRepository : IReferralStatsRepository
{
    private readonly ReferralDbContext _context;

    public ReferralStatsRepository(ReferralDbContext context)
    {
        _context = context;
    }

    public async Task<ReferralStats?> GetByCustomerIdAsync(Guid tenantId, Guid customerId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralStats
            .FirstOrDefaultAsync(r => r.CustomerId == customerId, cancellationToken);
    }

    public async Task<ReferralStats?> GetByProfessionalIdAsync(Guid tenantId, Guid professionalId, CancellationToken cancellationToken = default)
    {
        return await _context.ReferralStats
            .FirstOrDefaultAsync(r => r.ProfessionalId == professionalId, cancellationToken);
    }

    public async Task AddAsync(ReferralStats stats, CancellationToken cancellationToken = default)
    {
        await _context.ReferralStats.AddAsync(stats, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public void Update(ReferralStats stats)
    {
        _context.ReferralStats.Update(stats);
    }
}
