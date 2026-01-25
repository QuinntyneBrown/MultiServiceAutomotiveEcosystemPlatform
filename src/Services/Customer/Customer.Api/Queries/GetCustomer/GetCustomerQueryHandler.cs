using Customer.Core.Interfaces;
using MediatR;
using Shared.Contracts.DTOs;

namespace Customer.Api.Queries.GetCustomer;

public class GetCustomerQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto?>
{
    private readonly ICustomerRepository _repository;

    public GetCustomerQueryHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<CustomerDto?> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return customer is null ? null : MapToDto(customer);
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
