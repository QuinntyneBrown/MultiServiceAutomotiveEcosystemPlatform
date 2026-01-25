using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Core.Services;
using Shared.Contracts.DTOs;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.GenerateReferralCode;

public class GenerateReferralCodeCommandHandler : IRequestHandler<GenerateReferralCodeCommand, ReferralCodeDto>
{
    private readonly IReferralCodeRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly ILogger<GenerateReferralCodeCommandHandler> _logger;

    public GenerateReferralCodeCommandHandler(
        IReferralCodeRepository repository,
        ITenantContext tenantContext,
        ILogger<GenerateReferralCodeCommandHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _logger = logger;
    }

    public async Task<ReferralCodeDto> Handle(GenerateReferralCodeCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        // Generate unique code
        string code;
        do
        {
            code = ReferralCodeGenerator.Generate();
        } while (await _repository.CodeExistsAsync(_tenantContext.TenantId, code, cancellationToken));

        var referralCode = ReferralCode.Create(
            _tenantContext.TenantId,
            code,
            request.CustomerId,
            request.ProfessionalId,
            request.CampaignName,
            request.ExpiresAt,
            request.MaxUses);

        await _repository.AddAsync(referralCode, cancellationToken);

        _logger.LogInformation("Referral code {Code} generated for tenant {TenantId}",
            code, _tenantContext.TenantId);

        return MapToDto(referralCode);
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
