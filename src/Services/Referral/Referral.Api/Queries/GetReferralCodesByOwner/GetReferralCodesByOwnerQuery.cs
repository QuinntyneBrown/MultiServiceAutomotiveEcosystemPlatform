using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Queries.GetReferralCodesByOwner;

public record GetReferralCodesByOwnerQuery(Guid? CustomerId, Guid? ProfessionalId) : IRequest<IReadOnlyList<ReferralCodeDto>>;
