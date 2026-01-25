using MediatR;
using Shared.Contracts.DTOs;

namespace Referral.Api.Queries.GetReferralCode;

public record GetReferralCodeQuery(string Code) : IRequest<ReferralCodeDto?>;
