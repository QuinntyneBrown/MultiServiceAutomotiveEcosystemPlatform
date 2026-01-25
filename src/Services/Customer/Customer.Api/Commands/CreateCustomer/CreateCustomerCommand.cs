using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;

namespace Customer.Api.Commands.CreateCustomer;

public record CreateCustomerCommand(
    string Email,
    string? Phone,
    string FirstName,
    string LastName,
    string? MiddleName,
    DateTime? DateOfBirth,
    string? StreetAddress,
    string? City,
    string? State,
    string? PostalCode,
    string? Country,
    PreferredContactMethod PreferredContactMethod,
    bool MarketingOptIn,
    bool SmsOptIn
) : IRequest<CustomerDto>;
