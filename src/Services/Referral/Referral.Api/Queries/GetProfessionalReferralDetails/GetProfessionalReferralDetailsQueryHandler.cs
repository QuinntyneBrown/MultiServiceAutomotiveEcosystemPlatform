using MediatR;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetProfessionalReferralDetails;

public class GetProfessionalReferralDetailsQueryHandler : IRequestHandler<GetProfessionalReferralDetailsQuery, ProfessionalReferralDto?>
{
    private readonly IProfessionalReferralRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetProfessionalReferralDetailsQueryHandler(IProfessionalReferralRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ProfessionalReferralDto?> Handle(GetProfessionalReferralDetailsQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return referral is null ? null : MapToDto(referral);
    }

    private static ProfessionalReferralDto MapToDto(Core.Entities.ProfessionalReferral referral) => new()
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
        AcceptedAt = referral.AcceptedAt,
        CompletedAt = referral.CompletedAt
    };
}
