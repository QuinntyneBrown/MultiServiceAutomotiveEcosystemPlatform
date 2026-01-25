using MassTransit;
using MediatR;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.DeclineProfessionalReferral;

public class DeclineProfessionalReferralCommandHandler : IRequestHandler<DeclineProfessionalReferralCommand, ProfessionalReferralDto>
{
    private readonly IProfessionalReferralRepository _repository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DeclineProfessionalReferralCommandHandler> _logger;

    public DeclineProfessionalReferralCommandHandler(
        IProfessionalReferralRepository repository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<DeclineProfessionalReferralCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalReferralDto> Handle(DeclineProfessionalReferralCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional referral with ID {request.Id} not found");

        referral.Decline(request.Reason);

        _repository.Update(referral);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new ProfessionalReferralStatusChanged(
            referral.Id,
            referral.TenantId,
            "Pending",
            referral.Status.ToString(),
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Declined professional referral {ReferralId}", referral.Id);

        return MapToDto(referral);
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
