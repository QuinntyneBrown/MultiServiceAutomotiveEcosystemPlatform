using MediatR;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetCustomerReferralDetails;

public class GetCustomerReferralDetailsQueryHandler : IRequestHandler<GetCustomerReferralDetailsQuery, CustomerReferralDto?>
{
    private readonly ICustomerReferralRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetCustomerReferralDetailsQueryHandler(ICustomerReferralRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<CustomerReferralDto?> Handle(GetCustomerReferralDetailsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return referral is null ? null : MapToDto(referral);
    }

    private static CustomerReferralDto MapToDto(Core.Entities.CustomerReferral referral) => new()
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
