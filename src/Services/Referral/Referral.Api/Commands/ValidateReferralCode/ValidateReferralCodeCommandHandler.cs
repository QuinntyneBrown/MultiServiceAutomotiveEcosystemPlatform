using MediatR;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.ValidateReferralCode;

public class ValidateReferralCodeCommandHandler : IRequestHandler<ValidateReferralCodeCommand, ReferralCodeValidationResult>
{
    private readonly IReferralCodeRepository _repository;
    private readonly ITenantContext _tenantContext;

    public ValidateReferralCodeCommandHandler(IReferralCodeRepository repository, ITenantContext tenantContext)
    {
        _repository = repository;
        _tenantContext = tenantContext;
    }

    public async Task<ReferralCodeValidationResult> Handle(ValidateReferralCodeCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var code = await _repository.GetByCodeAsync(_tenantContext.TenantId, request.Code, cancellationToken);

        if (code is null)
        {
            return new ReferralCodeValidationResult(false, "Code not found", null);
        }

        if (!code.IsActive)
        {
            return new ReferralCodeValidationResult(false, "Code is inactive", MapToDto(code));
        }

        if (code.ExpiresAt.HasValue && code.ExpiresAt.Value < DateTime.UtcNow)
        {
            return new ReferralCodeValidationResult(false, "Code has expired", MapToDto(code));
        }

        if (code.MaxUses.HasValue && code.CurrentUses >= code.MaxUses.Value)
        {
            return new ReferralCodeValidationResult(false, "Code has reached maximum uses", MapToDto(code));
        }

        return new ReferralCodeValidationResult(true, "Code is valid", MapToDto(code));
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
