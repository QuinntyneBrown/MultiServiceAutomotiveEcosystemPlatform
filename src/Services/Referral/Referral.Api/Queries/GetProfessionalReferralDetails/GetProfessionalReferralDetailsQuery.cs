using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Queries.GetProfessionalReferralDetails;

public record GetProfessionalReferralDetailsQuery(Guid Id) : IRequest<ProfessionalReferralDto?>;
