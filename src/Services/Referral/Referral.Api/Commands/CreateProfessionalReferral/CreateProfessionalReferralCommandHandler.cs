using MassTransit;
using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.CreateProfessionalReferral;

public class CreateProfessionalReferralCommandHandler : IRequestHandler<CreateProfessionalReferralCommand, ProfessionalReferralDto>
{
    private readonly IProfessionalReferralRepository _repository;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateProfessionalReferralCommandHandler> _logger;

    public CreateProfessionalReferralCommandHandler(
        IProfessionalReferralRepository repository,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateProfessionalReferralCommandHandler> logger)
    {
        _repository = repository;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<ProfessionalReferralDto> Handle(CreateProfessionalReferralCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = ProfessionalReferral.Create(
            _tenantContext.TenantId,
            request.ReferringProfessionalId,
            request.ReferredBusinessName,
            request.ReferredEmail,
            request.ReferredPhone,
            request.ContactPersonName,
            request.ReferralSource,
            request.ReferralCode,
            request.Notes);

        await _repository.AddAsync(referral, cancellationToken);

        await _publishEndpoint.Publish(new ProfessionalReferralCreated(
            referral.Id,
            referral.TenantId,
            referral.ReferringProfessionalId,
            referral.ReferredBusinessName,
            referral.ReferredEmail,
            referral.CreatedAt), cancellationToken);

        _logger.LogInformation("Professional referral {ReferralId} created by professional {ProfessionalId}",
            referral.Id, referral.ReferringProfessionalId);

        return MapToDto(referral);
    }

    private static ProfessionalReferralDto MapToDto(ProfessionalReferral referral) => new()
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
        ContactedAt = referral.ContactedAt,
        ConvertedAt = referral.ConvertedAt
    };
}
