using MassTransit;
using MediatR;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.UseReferralCode;

public class UseReferralCodeCommandHandler : IRequestHandler<UseReferralCodeCommand, ReferralCodeDto>
{
    private readonly IReferralCodeRepository _repository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UseReferralCodeCommandHandler> _logger;

    public UseReferralCodeCommandHandler(
        IReferralCodeRepository repository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<UseReferralCodeCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ReferralCodeDto> Handle(UseReferralCodeCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var code = await _repository.GetByCodeAsync(_tenantContext.TenantId, request.Code, cancellationToken)
            ?? throw new KeyNotFoundException($"Referral code '{request.Code}' not found");

        if (!code.IsValid())
        {
            throw new InvalidOperationException("Code is not valid for use");
        }

        code.RecordUsage();

        _repository.Update(code);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event - UsedById would typically come from a user context
        // For now, use the owner ID if available
        var usedById = code.CustomerId ?? code.ProfessionalId ?? Guid.Empty;
        var @event = new ReferralCodeUsed(
            code.Id,
            code.TenantId,
            code.Code,
            usedById,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Recorded usage of referral code {Code} (use #{UseCount})", code.Code, code.CurrentUses);

        return MapToDto(code);
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
