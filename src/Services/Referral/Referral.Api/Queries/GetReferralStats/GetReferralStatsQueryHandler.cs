using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetReferralStats;

public class GetReferralStatsQueryHandler : IRequestHandler<GetReferralStatsQuery, ReferralStatsDto?>
{
    private readonly IReferralStatsRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetReferralStatsQueryHandler(IReferralStatsRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ReferralStatsDto?> Handle(GetReferralStatsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        ReferralStats? stats = null;

        if (request.CustomerId.HasValue)
        {
            stats = await _repository.GetByCustomerIdAsync(_tenantContext.TenantId, request.CustomerId.Value, cancellationToken);
        }
        else if (request.ProfessionalId.HasValue)
        {
            stats = await _repository.GetByProfessionalIdAsync(_tenantContext.TenantId, request.ProfessionalId.Value, cancellationToken);
        }

        return stats is null ? null : MapToDto(stats);
    }

    private static ReferralStatsDto MapToDto(ReferralStats stats) => new()
    {
        Id = stats.Id,
        TenantId = stats.TenantId,
        CustomerId = stats.CustomerId,
        ProfessionalId = stats.ProfessionalId,
        TotalReferrals = stats.TotalReferrals,
        SuccessfulReferrals = stats.SuccessfulReferrals,
        PendingReferrals = stats.PendingReferrals,
        TotalRewardsEarned = stats.TotalRewardsEarned,
        LastReferralAt = stats.LastReferralAt,
        LastSuccessfulReferralAt = stats.LastSuccessfulReferralAt
    };
}
