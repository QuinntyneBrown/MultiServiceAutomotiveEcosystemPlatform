using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.ConvertCustomerReferral;

public record ConvertCustomerReferralCommand(
    Guid ReferralId,
    Guid NewCustomerId,
    decimal? RewardAmount,
    string? RewardType
) : IRequest<CustomerReferralDto>;
