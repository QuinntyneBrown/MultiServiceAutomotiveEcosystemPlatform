using MassTransit;
using MediatR;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.ConvertCustomerReferral;

public class ConvertCustomerReferralCommandHandler : IRequestHandler<ConvertCustomerReferralCommand, CustomerReferralDto>
{
    private readonly ICustomerReferralRepository _repository;
    private readonly IReferralStatsRepository _statsRepository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ConvertCustomerReferralCommandHandler> _logger;

    public ConvertCustomerReferralCommandHandler(
        ICustomerReferralRepository repository,
        IReferralStatsRepository statsRepository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<ConvertCustomerReferralCommandHandler> logger)
    {
        _repository = repository;
        _statsRepository = statsRepository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerReferralDto> Handle(ConvertCustomerReferralCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = await _repository.GetByIdAsync(request.ReferralId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer referral with ID {request.ReferralId} not found");

        referral.Convert(request.NewCustomerId, request.RewardAmount, request.RewardType);

        // Update stats
        var stats = await _statsRepository.GetByCustomerIdAsync(_tenantContext.TenantId, referral.ReferringCustomerId, cancellationToken);
        stats?.RecordSuccessfulReferral(request.RewardAmount ?? 0);

        _repository.Update(referral);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerReferralConverted(
            referral.Id,
            referral.TenantId,
            request.NewCustomerId,
            request.RewardAmount,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Converted customer referral {ReferralId} with new customer {CustomerId}",
            referral.Id, request.NewCustomerId);

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
