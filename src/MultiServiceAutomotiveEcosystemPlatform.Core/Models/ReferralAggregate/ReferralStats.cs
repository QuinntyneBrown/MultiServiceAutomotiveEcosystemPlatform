using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

public class ReferralStats
{
    public Guid ReferralStatsId { get; private set; }
    public Guid TenantId { get; private set; }
    public ReferralEntityType EntityType { get; private set; }
    public Guid EntityId { get; private set; }

    // Customer Stats
    public int TotalReferralsSent { get; private set; }
    public int SuccessfulReferrals { get; private set; }
    public int PendingReferrals { get; private set; }
    public decimal TotalRewardsEarned { get; private set; }
    public decimal RewardsPending { get; private set; }

    // Professional Stats
    public int ReferralsReceived { get; private set; }
    public int ReferralsGiven { get; private set; }
    public decimal ReferralConversionRate { get; private set; }
    public decimal AvgDiscountGiven { get; private set; }

    // Timestamps
    public DateTime CalculatedAt { get; private set; }

    private ReferralStats() { }

    public ReferralStats(
        Guid tenantId,
        ReferralEntityType entityType,
        Guid entityId)
    {
        ReferralStatsId = Guid.NewGuid();
        TenantId = tenantId;
        EntityType = entityType;
        EntityId = entityId;
        CalculatedAt = DateTime.UtcNow;
    }

    public void UpdateCustomerStats(
        int totalReferralsSent,
        int successfulReferrals,
        int pendingReferrals,
        decimal totalRewardsEarned,
        decimal rewardsPending)
    {
        if (EntityType != ReferralEntityType.Customer)
            throw new InvalidOperationException("Cannot update customer stats for a professional entity.");

        TotalReferralsSent = totalReferralsSent;
        SuccessfulReferrals = successfulReferrals;
        PendingReferrals = pendingReferrals;
        TotalRewardsEarned = totalRewardsEarned;
        RewardsPending = rewardsPending;
        CalculatedAt = DateTime.UtcNow;
    }

    public void UpdateProfessionalStats(
        int referralsReceived,
        int referralsGiven,
        decimal referralConversionRate,
        decimal avgDiscountGiven)
    {
        if (EntityType != ReferralEntityType.Professional)
            throw new InvalidOperationException("Cannot update professional stats for a customer entity.");

        ReferralsReceived = referralsReceived;
        ReferralsGiven = referralsGiven;
        ReferralConversionRate = referralConversionRate;
        AvgDiscountGiven = avgDiscountGiven;
        CalculatedAt = DateTime.UtcNow;
    }

    public void IncrementReferralSent()
    {
        TotalReferralsSent++;
        PendingReferrals++;
        CalculatedAt = DateTime.UtcNow;
    }

    public void IncrementSuccessfulReferral(decimal rewardAmount)
    {
        SuccessfulReferrals++;
        if (PendingReferrals > 0)
            PendingReferrals--;
        RewardsPending += rewardAmount;
        CalculatedAt = DateTime.UtcNow;
    }

    public void MarkRewardPaid(decimal rewardAmount)
    {
        if (RewardsPending >= rewardAmount)
            RewardsPending -= rewardAmount;
        TotalRewardsEarned += rewardAmount;
        CalculatedAt = DateTime.UtcNow;
    }

    public void RecalculateConversionRate()
    {
        if (EntityType == ReferralEntityType.Professional && ReferralsReceived > 0)
        {
            // Calculate based on completed vs received
            // This is a simplified calculation - in real scenario you'd query the database
            CalculatedAt = DateTime.UtcNow;
        }
    }
}
