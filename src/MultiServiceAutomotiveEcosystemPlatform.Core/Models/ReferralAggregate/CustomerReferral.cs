using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

public class CustomerReferral
{
    public Guid CustomerReferralId { get; private set; }
    public Guid TenantId { get; private set; }

    // Referrer Information
    public Guid ReferrerCustomerId { get; private set; }
    public string ReferrerCode { get; private set; } = string.Empty;

    // Referee Information
    public Guid? RefereeCustomerId { get; private set; }
    public string? RefereeEmail { get; private set; }
    public string? RefereePhone { get; private set; }
    public string? RefereeName { get; private set; }

    // Target
    public Guid? TargetProfessionalId { get; private set; }
    public string? TargetServiceType { get; private set; }

    // Status
    public CustomerReferralStatus Status { get; private set; }
    public DateTime? ConvertedAt { get; private set; }

    // Reward
    public RewardStatus RewardStatus { get; private set; }
    public decimal? RewardAmount { get; private set; }
    public RewardType RewardType { get; private set; }
    public DateTime? RewardPaidAt { get; private set; }

    // Tracking
    public string? ReferralSource { get; private set; }
    public string? UtmCampaign { get; private set; }
    public string? UtmSource { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private CustomerReferral() { }

    public CustomerReferral(
        Guid tenantId,
        Guid referrerCustomerId,
        string referrerCode,
        string? refereeEmail = null,
        string? refereePhone = null,
        string? refereeName = null,
        Guid? targetProfessionalId = null,
        string? targetServiceType = null,
        string? referralSource = null,
        int expirationDays = 90)
    {
        if (string.IsNullOrWhiteSpace(referrerCode))
            throw new ArgumentException("Referrer code cannot be empty.", nameof(referrerCode));

        if (string.IsNullOrWhiteSpace(refereeEmail) && string.IsNullOrWhiteSpace(refereePhone))
            throw new ArgumentException("Either referee email or phone must be provided.");

        CustomerReferralId = Guid.NewGuid();
        TenantId = tenantId;
        ReferrerCustomerId = referrerCustomerId;
        ReferrerCode = referrerCode;
        RefereeEmail = refereeEmail?.ToLowerInvariant().Trim();
        RefereePhone = refereePhone != null ? NormalizePhone(refereePhone) : null;
        RefereeName = refereeName?.Trim();
        TargetProfessionalId = targetProfessionalId;
        TargetServiceType = targetServiceType?.Trim();
        Status = CustomerReferralStatus.Pending;
        RewardStatus = RewardStatus.Pending;
        RewardType = RewardType.Cash;
        ReferralSource = referralSource?.Trim();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expirationDays);
    }

    public void MarkContacted()
    {
        if (Status != CustomerReferralStatus.Pending)
            throw new InvalidOperationException("Can only mark as contacted from pending status.");

        Status = CustomerReferralStatus.Contacted;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Convert(Guid refereeCustomerId, decimal? rewardAmount = null, RewardType rewardType = RewardType.Cash)
    {
        if (Status == CustomerReferralStatus.Converted)
            throw new InvalidOperationException("Referral has already been converted.");
        if (Status == CustomerReferralStatus.Expired || Status == CustomerReferralStatus.Cancelled)
            throw new InvalidOperationException("Cannot convert an expired or cancelled referral.");

        RefereeCustomerId = refereeCustomerId;
        Status = CustomerReferralStatus.Converted;
        ConvertedAt = DateTime.UtcNow;
        RewardAmount = rewardAmount;
        RewardType = rewardType;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApproveReward()
    {
        if (Status != CustomerReferralStatus.Converted)
            throw new InvalidOperationException("Can only approve reward for converted referrals.");
        if (RewardStatus != RewardStatus.Pending)
            throw new InvalidOperationException("Reward is not in pending status.");

        RewardStatus = RewardStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkRewardPaid()
    {
        if (RewardStatus != RewardStatus.Approved)
            throw new InvalidOperationException("Can only mark as paid if reward is approved.");

        RewardStatus = RewardStatus.Paid;
        RewardPaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CancelReward()
    {
        RewardStatus = RewardStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status == CustomerReferralStatus.Converted)
            throw new InvalidOperationException("Cannot expire a converted referral.");

        Status = CustomerReferralStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == CustomerReferralStatus.Converted)
            throw new InvalidOperationException("Cannot cancel a converted referral.");

        Status = CustomerReferralStatus.Cancelled;
        RewardStatus = RewardStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetTrackingInfo(string? utmCampaign, string? utmSource)
    {
        UtmCampaign = utmCampaign?.Trim();
        UtmSource = utmSource?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value && Status == CustomerReferralStatus.Pending;
    public bool IsConverted => Status == CustomerReferralStatus.Converted;
    public bool IsPending => Status == CustomerReferralStatus.Pending || Status == CustomerReferralStatus.Contacted;

    private static string NormalizePhone(string phone)
    {
        return new string(phone.Where(char.IsDigit).ToArray());
    }
}
