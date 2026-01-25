using MassTransit;
using MediatR;
using Referral.Core.Entities;
using Referral.Core.Interfaces;
using Referral.Infrastructure.Data;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Referral;
using Shared.Infrastructure.Services;

namespace Referral.Api.Commands.CreateCustomerReferral;

public class CreateCustomerReferralCommandHandler : IRequestHandler<CreateCustomerReferralCommand, CustomerReferralDto>
{
    private readonly ICustomerReferralRepository _repository;
    private readonly IReferralStatsRepository _statsRepository;
    private readonly ReferralDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateCustomerReferralCommandHandler> _logger;

    public CreateCustomerReferralCommandHandler(
        ICustomerReferralRepository repository,
        IReferralStatsRepository statsRepository,
        ReferralDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateCustomerReferralCommandHandler> logger)
    {
        _repository = repository;
        _statsRepository = statsRepository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerReferralDto> Handle(CreateCustomerReferralCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var referral = new CustomerReferral(
            _tenantContext.TenantId,
            request.ReferringCustomerId,
            request.ReferredEmail,
            request.ReferredPhone,
            request.ReferredFirstName,
            request.ReferredLastName,
            request.ReferralSource,
            request.ReferralCode,
            request.Notes);

        await _repository.AddAsync(referral, cancellationToken);

        // Update or create stats
        var stats = await _statsRepository.GetByOwnerAsync(_tenantContext.TenantId, request.ReferringCustomerId, cancellationToken);
        if (stats is null)
        {
            stats = new ReferralStats(_tenantContext.TenantId, request.ReferringCustomerId, "Customer");
            await _statsRepository.AddAsync(stats, cancellationToken);
        }
        stats.RecordNewReferral();

        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerReferralCreated(
            referral.Id,
            referral.TenantId,
            referral.ReferringCustomerId,
            referral.ReferredEmail,
            referral.ReferredFirstName,
            referral.ReferredLastName,
            referral.ReferralCode,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Created customer referral {ReferralId} for tenant {TenantId}", referral.Id, referral.TenantId);

        return MapToDto(referral);
    }

    private static CustomerReferralDto MapToDto(CustomerReferral referral) => new()
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
