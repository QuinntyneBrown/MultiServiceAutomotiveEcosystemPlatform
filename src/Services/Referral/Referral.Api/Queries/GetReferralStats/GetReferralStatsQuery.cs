using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Queries.GetReferralStats;

public record GetReferralStatsQuery(Guid? CustomerId, Guid? ProfessionalId) : IRequest<ReferralStatsDto?>;
