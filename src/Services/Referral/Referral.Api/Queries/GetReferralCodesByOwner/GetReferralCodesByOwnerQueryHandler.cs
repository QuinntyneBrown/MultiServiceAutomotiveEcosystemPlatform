using MediatR;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetReferralCodesByOwner;

public class GetReferralCodesByOwnerQueryHandler : IRequestHandler<GetReferralCodesByOwnerQuery, IReadOnlyList<ReferralCodeDto>>
{
    private readonly IReferralCodeRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetReferralCodesByOwnerQueryHandler(IReferralCodeRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<IReadOnlyList<ReferralCodeDto>> Handle(GetReferralCodesByOwnerQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        if (!request.CustomerId.HasValue && !request.ProfessionalId.HasValue)
            throw new ArgumentException("Either CustomerId or ProfessionalId must be provided");

        IReadOnlyList<Core.Entities.ReferralCode> codes;

        if (request.CustomerId.HasValue)
        {
            codes = await _repository.GetByCustomerIdAsync(_tenantContext.TenantId, request.CustomerId.Value, cancellationToken);
        }
        else
        {
            codes = await _repository.GetByProfessionalIdAsync(_tenantContext.TenantId, request.ProfessionalId!.Value, cancellationToken);
        }

        return codes.Select(MapToDto).ToList();
    }

    private static ReferralCodeDto MapToDto(Core.Entities.ReferralCode code) => new()
    {
        Id = code.Id,
        TenantId = code.TenantId,
        Code = code.Code,
        CustomerId = code.CustomerId,
        ProfessionalId = code.ProfessionalId,
        CampaignName = code.CampaignName,
        MaxUses = code.MaxUses,
        CurrentUses = code.CurrentUses,
        ExpiresAt = code.ExpiresAt,
        IsActive = code.IsActive,
        IsValid = code.IsValid(),
        CreatedAt = code.CreatedAt
    };
}
