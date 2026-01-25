using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.DeclineProfessionalReferral;

public record DeclineProfessionalReferralCommand(Guid Id, string? Reason) : IRequest<ProfessionalReferralDto>;
