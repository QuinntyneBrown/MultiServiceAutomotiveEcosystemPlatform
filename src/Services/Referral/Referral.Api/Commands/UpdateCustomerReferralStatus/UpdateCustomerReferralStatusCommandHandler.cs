using MassTransit;
using MediatR;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.UpdateCustomerReferralStatus;

public class UpdateCustomerReferralStatusCommandHandler : IRequestHandler<UpdateCustomerReferralStatusCommand, CustomerReferralDto>
{
    private readonly ICustomerReferralRepository _repository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateCustomerReferralStatusCommandHandler> _logger;

    public UpdateCustomerReferralStatusCommandHandler(
        ICustomerReferralRepository repository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateCustomerReferralStatusCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerReferralDto> Handle(UpdateCustomerReferralStatusCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer referral with ID {request.Id} not found");

        var previousStatus = referral.Status;

        switch (request.Status)
        {
            case CustomerReferralStatus.Contacted:
                referral.MarkContacted();
                break;
            case CustomerReferralStatus.Expired:
                referral.Expire();
                break;
            case CustomerReferralStatus.Cancelled:
                referral.Cancel();
                break;
            default:
                throw new ArgumentException($"Cannot transition to status: {request.Status}");
        }

        _repository.Update(referral);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerReferralStatusChanged(
            referral.Id,
            referral.TenantId,
            previousStatus.ToString(),
            referral.Status.ToString(),
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated customer referral {ReferralId} status from {PreviousStatus} to {NewStatus}",
            referral.Id, previousStatus, referral.Status);

        return MapToDto(referral);
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
