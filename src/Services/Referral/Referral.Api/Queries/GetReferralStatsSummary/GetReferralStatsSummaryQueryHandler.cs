using MediatR;
using Microsoft.EntityFrameworkCore;
using Referral.Infrastructure.Data;
using Shared.Contracts.Enums;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetReferralStatsSummary;

public class GetReferralStatsSummaryQueryHandler : IRequestHandler<GetReferralStatsSummaryQuery, ReferralStatsSummaryDto>
{
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;

    public GetReferralStatsSummaryQueryHandler(ReferralDbContext context, ITenantContext tenantContext)
    {
        _context = context;
        _tenantContext = tenantContext;
    }

    public async Task<ReferralStatsSummaryDto> Handle(GetReferralStatsSummaryQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var tenantId = _tenantContext.TenantId;

        var totalCustomerReferrals = await _context.CustomerReferrals
            .CountAsync(cancellationToken);

        var totalProfessionalReferrals = await _context.ProfessionalReferrals
            .CountAsync(cancellationToken);

        var totalConversions = await _context.CustomerReferrals
            .CountAsync(r => r.Status == CustomerReferralStatus.Converted, cancellationToken);

        totalConversions += await _context.ProfessionalReferrals
            .CountAsync(r => r.Status == ProfessionalReferralStatus.Completed, cancellationToken);

        var activeReferralCodes = await _context.ReferralCodes
            .CountAsync(c => c.IsActive, cancellationToken);

        var totalRewardsApproved = await _context.CustomerReferrals
            .Where(r => r.RewardStatus == RewardStatus.Approved || r.RewardStatus == RewardStatus.Paid)
            .SumAsync(r => r.RewardAmount ?? 0, cancellationToken);

        totalRewardsApproved += await _context.ProfessionalReferrals
            .Where(r => r.RewardStatus == RewardStatus.Approved || r.RewardStatus == RewardStatus.Paid)
            .SumAsync(r => r.RewardAmount ?? 0, cancellationToken);

        var totalRewardsPaid = await _context.CustomerReferrals
            .Where(r => r.RewardStatus == RewardStatus.Paid)
            .SumAsync(r => r.RewardAmount ?? 0, cancellationToken);

        totalRewardsPaid += await _context.ProfessionalReferrals
            .Where(r => r.RewardStatus == RewardStatus.Paid)
            .SumAsync(r => r.RewardAmount ?? 0, cancellationToken);

        return new ReferralStatsSummaryDto
        {
            TotalCustomerReferrals = totalCustomerReferrals,
            TotalProfessionalReferrals = totalProfessionalReferrals,
            TotalConversions = totalConversions,
            ActiveReferralCodes = activeReferralCodes,
            TotalRewardsApproved = totalRewardsApproved,
            TotalRewardsPaid = totalRewardsPaid
        };
    }
}
