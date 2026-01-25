using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.Enums;

namespace Referral.Api.Commands.UpdateCustomerReferralStatus;

public record UpdateCustomerReferralStatusCommand(Guid Id, CustomerReferralStatus Status) : IRequest<CustomerReferralDto>;
