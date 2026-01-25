using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Queries.GetReferralCode;

public class GetReferralCodeQueryHandler : IRequestHandler<GetReferralCodeQuery, ReferralCodeDto?>
{
    private readonly IReferralCodeRepository _repository;
    private readonly ITenantContext _tenantContext;

    public GetReferralCodeQueryHandler(IReferralCodeRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ReferralCodeDto?> Handle(GetReferralCodeQuery request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var code = await _repository.GetByCodeAsync(_tenantContext.TenantId, request.Code, cancellationToken);

        return code is null ? null : MapToDto(code);
    }

    private static ReferralCodeDto MapToDto(ReferralCode code) => new()
    {
        Id = code.Id,
        TenantId = code.TenantId,
        Code = code.Code,
        CustomerId = code.CustomerId,
        ProfessionalId = code.ProfessionalId,
        CampaignName = code.CampaignName,
        IsActive = code.IsActive,
        ExpiresAt = code.ExpiresAt,
        MaxUses = code.MaxUses,
        CurrentUses = code.CurrentUses,
        CreatedAt = code.CreatedAt
    };
}
