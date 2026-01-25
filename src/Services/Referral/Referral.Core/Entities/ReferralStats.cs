using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ReferralStats : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid OwnerId { get; private set; }
    public string OwnerType { get; private set; } = string.Empty; // Customer, Professional
    public int TotalReferrals { get; private set; }
    public int SuccessfulReferrals { get; private set; }
    public int PendingReferrals { get; private set; }
    public decimal TotalRewardsEarned { get; private set; }
    public decimal TotalRewardsPaid { get; private set; }
    public DateTime? LastReferralAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ReferralStats() { }

    public ReferralStats(Guid tenantId, Guid ownerId, string ownerType)
    {
        Id = Guid.NewGuid();
        TenantId = tenantId;
        OwnerId = ownerId;
        OwnerType = ownerType;
        CreatedAt = DateTime.UtcNow;
    }

    public void RecordNewReferral()
    {
        TotalReferrals++;
        PendingReferrals++;
        LastReferralAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordSuccessfulReferral(decimal rewardAmount)
    {
        SuccessfulReferrals++;
        PendingReferrals = Math.Max(0, PendingReferrals - 1);
        TotalRewardsEarned += rewardAmount;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedReferral()
    {
        PendingReferrals = Math.Max(0, PendingReferrals - 1);
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordRewardPaid(decimal amount)
    {
        TotalRewardsPaid += amount;
        UpdatedAt = DateTime.UtcNow;
    }
}
