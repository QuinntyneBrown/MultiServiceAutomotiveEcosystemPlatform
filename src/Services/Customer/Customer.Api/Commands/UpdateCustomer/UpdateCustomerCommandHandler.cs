using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Customer;
using Shared.Infrastructure.Services;

namespace Customer.Api.Commands.UpdateCustomer;

public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<UpdateCustomerCommandHandler> _logger;

    public UpdateCustomerCommandHandler(
        ICustomerRepository repository,
        CustomerDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<UpdateCustomerCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerDto> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.Id} not found");

        // Check for duplicate email (excluding current customer)
        if (await _repository.EmailExistsAsync(_tenantContext.TenantId, request.Email, request.Id, cancellationToken))
        {
            throw new InvalidOperationException($"A customer with email '{request.Email}' already exists");
        }

        customer.UpdateContactInfo(request.Email, request.Phone, request.PreferredContactMethod);
        customer.UpdatePersonalInfo(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);
        customer.UpdateAddress(request.StreetAddress, request.City, request.State, request.PostalCode, request.Country);
        customer.UpdatePreferences(request.MarketingOptIn, request.SmsOptIn);

        _repository.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerUpdated(
            customer.Id,
            customer.TenantId,
            customer.Email,
            customer.FirstName,
            customer.LastName,
            customer.Phone,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Updated customer {CustomerId} for tenant {TenantId}", customer.Id, customer.TenantId);

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
