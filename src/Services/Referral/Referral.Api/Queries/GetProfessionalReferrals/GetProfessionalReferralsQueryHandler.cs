using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetProfessionalReferrals;

public class GetProfessionalReferralsQueryHandler : IRequestHandler<GetProfessionalReferralsQuery, PagedResult<ProfessionalReferralDto>>
{
    private readonly IProfessionalReferralRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalReferralsQueryHandler(IProfessionalReferralRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<PagedResult<ProfessionalReferralDto>> Handle(GetProfessionalReferralsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (page - 1) * pageSize;

        var referrals = await _repository.GetByReferringProfessionalAsync(
            _tenantContext.TenantId, request.ProfessionalId, skip, pageSize, cancellationToken);
        var totalCount = await _repository.GetCountByReferringProfessionalAsync(
            _tenantContext.TenantId, request.ProfessionalId, cancellationToken);

        var items = referrals.Select(MapToDto).ToList();

        return PagedResult<ProfessionalReferralDto>.Create(items, totalCount, page, pageSize);
    }

    private static ProfessionalReferralDto MapToDto(ProfessionalReferral referral) => new()
    {
        Id = referral.Id,
        TenantId = referral.TenantId,
        ReferringProfessionalId = referral.ReferringProfessionalId,
        ReferredBusinessName = referral.ReferredBusinessName,
        ReferredEmail = referral.ReferredEmail,
        ReferredPhone = referral.ReferredPhone,
        ContactPersonName = referral.ContactPersonName,
        ConvertedProfessionalId = referral.ConvertedProfessionalId,
        Status = referral.Status,
        ReferralSource = referral.ReferralSource,
        ReferralCode = referral.ReferralCode,
        Notes = referral.Notes,
        RewardStatus = referral.RewardStatus,
        RewardAmount = referral.RewardAmount,
        RewardType = referral.RewardType,
        RewardPaidAt = referral.RewardPaidAt,
        CreatedAt = referral.CreatedAt,
        ContactedAt = referral.ContactedAt,
        ConvertedAt = referral.ConvertedAt
    };
}
