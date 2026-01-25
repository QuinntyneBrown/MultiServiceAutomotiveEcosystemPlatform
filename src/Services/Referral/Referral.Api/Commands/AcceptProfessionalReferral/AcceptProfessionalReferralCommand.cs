using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Commands.AcceptProfessionalReferral;

public record AcceptProfessionalReferralCommand(Guid Id) : IRequest<ProfessionalReferralDto>;
