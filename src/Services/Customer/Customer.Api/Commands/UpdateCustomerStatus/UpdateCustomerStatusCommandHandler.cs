using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;
using Shared.Contracts.Events.Customer;
using Shared.Infrastructure.Services;

namespace Customer.Api.Commands.UpdateCustomerStatus;

public class UpdateCustomerStatusCommandHandler : IRequestHandler<UpdateCustomerStatusCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateCustomerStatusCommandHandler> _logger;

    public UpdateCustomerStatusCommandHandler(
        ICustomerRepository repository,
        CustomerDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateCustomerStatusCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerStatusCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.Id} not found");

        var previousStatus = customer.Status;

        switch (request.Status)
        {
            case CustomerStatus.Active:
                customer.Activate();
                break;
            case CustomerStatus.Inactive:
                customer.Deactivate();
                break;
            case CustomerStatus.Blocked:
                customer.Block();
                break;
            default:
                throw new ArgumentException($"Invalid status: {request.Status}");
        }

        _repository.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerStatusChanged(
            customer.Id,
            customer.TenantId,
            previousStatus.ToString(),
            customer.Status.ToString(),
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated customer {CustomerId} status from {PreviousStatus} to {NewStatus}",
            customer.Id, previousStatus, customer.Status);

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
