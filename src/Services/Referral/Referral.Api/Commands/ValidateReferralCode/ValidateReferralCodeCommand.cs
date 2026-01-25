using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.ValidateReferralCode;

public record ValidateReferralCodeCommand(string Code) : IRequest<ReferralCodeValidationResult>;

public record ReferralCodeValidationResult(bool IsValid, string? Message, ReferralCodeDto? Code);
