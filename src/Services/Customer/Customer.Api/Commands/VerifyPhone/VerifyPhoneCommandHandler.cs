using Customer.Core.Interfaces;
using Customer.Infrastructure.Data;
using MassTransit;
using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Events.Customer;
using Shared.Infrastructure.Services;

namespace Customer.Api.Commands.VerifyPhone;

public class VerifyPhoneCommandHandler : IRequestHandler<VerifyPhoneCommand, CustomerDto>
{
    private readonly ICustomerRepository _repository;
    private readonly CustomerDbContext _context;
    private readonly ITenantContext _tenantContext;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<VerifyPhoneCommandHandler> _logger;

    public VerifyPhoneCommandHandler(
        ICustomerRepository repository,
        CustomerDbContext context,
        ITenantContext tenantContext,
        IPublishEndpoint publishEndpoint,
        ILogger<VerifyPhoneCommandHandler> logger)
    {
        _repository = repository;
        _context = context;
        _tenantContext = tenantContext;
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task<CustomerDto> Handle(VerifyPhoneCommand request, CancellationToken cancellationToken)
    {
        if (!_tenantContext.HasTenant)
            throw new InvalidOperationException("Tenant context is required");

        var customer = await _repository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new KeyNotFoundException($"Customer with ID {request.CustomerId} not found");

        customer.VerifyPhone();

        _repository.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);

        // Publish event
        var @event = new CustomerVerified(
            customer.Id,
            customer.TenantId,
            "Phone",
            DateTime.UtcNow);

        await _publishEndpoint.Publish(@event, cancellationToken);

        _logger.LogInformation("Verified phone for customer {CustomerId}", customer.Id);

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
