using MediatR;

namespace Referral.Api.Queries.GetReferralStatsSummary;

public record GetReferralStatsSummaryQuery() : IRequest<ReferralStatsSummaryDto>;

public record ReferralStatsSummaryDto
{
    public int TotalCustomerReferrals { get; init; }
    public int TotalProfessionalReferrals { get; init; }
    public int TotalConversions { get; init; }
    public int ActiveReferralCodes { get; init; }
    public decimal TotalRewardsApproved { get; init; }
    public decimal TotalRewardsPaid { get; init; }
}
