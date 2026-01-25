using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Customer;
using Shared.Infrastructure.Services;

namespace Customer.Api.Commands.TransferOwnership;

public class TransferOwnershipCommandHandler : IRequestHandler<TransferOwnershipCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<TransferOwnershipCommandHandler> _logger;

    public TransferOwnershipCommandHandler(
        ICustomerRepository repository,
        CustomerDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<TransferOwnershipCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerDto> Handle(TransferOwnershipCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");

        var previousOwnerId = customer.OwnerProfessionalId;
        customer.TransferOwnership(request.NewOwnerId, request.Reason);

        _repository.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerOwnershipChanged(
            customer.Id,
            customer.TenantId,
            previousOwnerId,
            request.NewOwnerId,
            request.Reason,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Transferred customer {CustomerId} from {PreviousOwner} to {NewOwner}",
            customer.Id, previousOwnerId, request.NewOwnerId);

        return MapToDto(customer);
    }

    private static CustomerDto MapToDto(Core.Entities.Customer customer) => new()
    {
        Id = customer.Id,
        TenantId = customer.TenantId,
        Email = customer.Email,
        Phone = customer.Phone,
        FirstName = customer.FirstName,
        LastName = customer.LastName,
        MiddleName = customer.MiddleName,
        DateOfBirth = customer.DateOfBirth,
        StreetAddress = customer.StreetAddress,
        City = customer.City,
        State = customer.State,
        PostalCode = customer.PostalCode,
        Country = customer.Country,
        PreferredContactMethod = customer.PreferredContactMethod,
        MarketingOptIn = customer.MarketingOptIn,
        SmsOptIn = customer.SmsOptIn,
        Status = customer.Status,
        EmailVerified = customer.EmailVerified,
        PhoneVerified = customer.PhoneVerified,
        OwnerProfessionalId = customer.OwnerProfessionalId,
        CreatedAt = customer.CreatedAt,
        UpdatedAt = customer.UpdatedAt
    };
}
