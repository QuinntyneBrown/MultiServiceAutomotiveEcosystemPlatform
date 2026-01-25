using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Customer;
using Shared.Infrastructure.Services;

namespace Customer.Api.Commands.CreateCustomer;

public class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<CreateCustomerCommandHandler> _logger;

    public CreateCustomerCommandHandler(
        ICustomerRepository repository,
        CustomerDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<CreateCustomerCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        // Check for duplicate email
        if (await _repository.EmailExistsAsync(_tenantContext.TenantId, request.Email, cancellationToken: cancellationToken))
        {
            throw new InvalidOperationException($"A customer with email '{request.Email}' already exists");
        }

        var customer = new Core.Entities.Customer(
            _tenantContext.TenantId,
            request.Email,
            request.Phone,
            request.FirstName,
            request.LastName);

        customer.UpdatePersonalInfo(request.FirstName, request.LastName, request.MiddleName, request.DateOfBirth);
        customer.UpdateAddress(request.StreetAddress, request.City, request.State, request.PostalCode, request.Country);
        customer.UpdateContactInfo(request.Email, request.Phone, request.PreferredContactMethod);
        customer.UpdatePreferences(request.MarketingOptIn, request.SmsOptIn);

        await _repository.AddAsync(customer, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerCreated(
            customer.Id,
            customer.TenantId,
            customer.Email,
            customer.FirstName,
            customer.LastName,
            customer.Phone,
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Created customer {CustomerId} for tenant {TenantId}", customer.Id, customer.TenantId);

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
