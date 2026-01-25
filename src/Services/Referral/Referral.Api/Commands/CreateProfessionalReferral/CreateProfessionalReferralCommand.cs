using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.CreateProfessionalReferral;

public record CreateProfessionalReferralCommand(
    Guid ReferringProfessionalId,
    string ReferredBusinessName,
    string ReferredEmail,
    string? ReferredPhone,
    string? ContactPersonName,
    string? ReferralSource,
    string? ReferralCode,
    string? Notes) : IRequest<ProfessionalReferralDto>;
