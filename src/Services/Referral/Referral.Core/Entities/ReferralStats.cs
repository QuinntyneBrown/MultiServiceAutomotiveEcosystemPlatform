using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ReferralStats : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid? CustomerId { get; private set; }
    public Guid? ProfessionalId { get; private set; }
    public int TotalReferrals { get; private set; }
    public int SuccessfulReferrals { get; private set; }
    public int PendingReferrals { get; private set; }
    public decimal TotalRewardsEarned { get; private set; }
    public DateTime? LastReferralAt { get; private set; }
    public DateTime? LastSuccessfulReferralAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ReferralStats() { }

    public static ReferralStats CreateForCustomer(Guid tenantId, Guid customerId)
    {
        return new ReferralStats
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            CustomerId = customerId,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static ReferralStats CreateForProfessional(Guid tenantId, Guid professionalId)
    {
        return new ReferralStats
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ProfessionalId = professionalId,
            CreatedAt = DateTime.UtcNow
        };
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
        LastSuccessfulReferralAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void RecordFailedReferral()
    {
        PendingReferrals = Math.Max(0, PendingReferrals - 1);
        UpdatedAt = DateTime.UtcNow;
    }
}
