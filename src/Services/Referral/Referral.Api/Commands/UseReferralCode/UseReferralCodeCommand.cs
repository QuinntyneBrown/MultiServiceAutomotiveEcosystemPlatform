using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.UseReferralCode;

public record UseReferralCodeCommand(string Code) : IRequest<ReferralCodeDto>;
