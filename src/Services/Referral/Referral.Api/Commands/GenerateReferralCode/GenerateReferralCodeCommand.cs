using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.GenerateReferralCode;

public record GenerateReferralCodeCommand(
    Guid? CustomerId,
    Guid? ProfessionalId,
    string? CampaignName,
    DateTime? ExpiresAt,
    int? MaxUses) : IRequest<ReferralCodeDto>;
