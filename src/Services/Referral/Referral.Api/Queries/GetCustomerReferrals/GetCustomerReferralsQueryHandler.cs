using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetCustomerReferrals;

public class GetCustomerReferralsQueryHandler : IRequestHandler<GetCustomerReferralsQuery, PagedResult<CustomerReferralDto>>
{
    private readonly ICustomerReferralRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetCustomerReferralsQueryHandler(ICustomerReferralRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<PagedResult<CustomerReferralDto>> Handle(GetCustomerReferralsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var page = Math.Max(1, request.Page);
        var pageSize = Math.Clamp(request.PageSize, 1, 100);
        var skip = (page - 1) * pageSize;

        var referrals = await _repository.GetByReferringCustomerAsync(
            _tenantContext.TenantId, request.CustomerId, skip, pageSize, cancellationToken);
        var totalCount = await _repository.GetCountByReferringCustomerAsync(
            _tenantContext.TenantId, request.CustomerId, cancellationToken);

        var items = referrals.Select(MapToDto).ToList();

        return PagedResult<CustomerReferralDto>.Create(items, totalCount, page, pageSize);
    }

    private static CustomerReferralDto MapToDto(CustomerReferral referral) => new()
    {
        Id = referral.Id,
        TenantId = referral.TenantId,
        ReferringCustomerId = referral.ReferringCustomerId,
        ReferredEmail = referral.ReferredEmail,
        ReferredPhone = referral.ReferredPhone,
        ReferredFirstName = referral.ReferredFirstName,
        ReferredLastName = referral.ReferredLastName,
        ConvertedCustomerId = referral.ConvertedCustomerId,
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
