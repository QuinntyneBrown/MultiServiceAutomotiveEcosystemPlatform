using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.CreateCustomerReferral;

public record CreateCustomerReferralCommand(
    Guid ReferringCustomerId,
    string ReferredEmail,
    string? ReferredPhone,
    string? ReferredFirstName,
    string? ReferredLastName,
    string? ReferralSource,
    string? ReferralCode,
    string? Notes
) : IRequest<CustomerReferralDto>;
