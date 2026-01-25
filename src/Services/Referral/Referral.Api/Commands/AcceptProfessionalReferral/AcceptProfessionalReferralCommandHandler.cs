using MassTransit;
using MediatR;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.AcceptProfessionalReferral;

public class AcceptProfessionalReferralCommandHandler : IRequestHandler<AcceptProfessionalReferralCommand, ProfessionalReferralDto>
{
    private readonly IProfessionalReferralRepository _repository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<AcceptProfessionalReferralCommandHandler> _logger;

    public AcceptProfessionalReferralCommandHandler(
        IProfessionalReferralRepository repository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<AcceptProfessionalReferralCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalReferralDto> Handle(AcceptProfessionalReferralCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Professional referral with ID {request.Id} not found");

        referral.Accept();

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

        _logger.LogInformation("Accepted professional referral {ReferralId}", referral.Id);

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
