using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Queries.GetCustomerReferralDetails;

public record GetCustomerReferralDetailsQuery(Guid Id) : IRequest<CustomerReferralDto?>;
