using MediatR;
using Shared.Contracts.DTOs;
using Shared.Contracts.DTOs.Common;

namespace Referral.Api.Queries.GetProfessionalReferrals;

public record GetProfessionalReferralsQuery(Guid ProfessionalId, int Page = 1, int PageSize = 10) : IRequest<PagedResult<ProfessionalReferralDto>>;
