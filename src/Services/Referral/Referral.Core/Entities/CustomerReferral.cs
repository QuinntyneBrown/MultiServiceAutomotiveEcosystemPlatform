using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class CustomerReferral : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ReferringCustomerId { get; private set; }
    public string ReferredEmail { get; private set; } = string.Empty;
    public string? ReferredPhone { get; private set; }
    public string? ReferredFirstName { get; private set; }
    public string? ReferredLastName { get; private set; }
    public Guid? ConvertedCustomerId { get; private set; }
    public CustomerReferralStatus Status { get; private set; }
    public string? ReferralSource { get; private set; }
    public string? ReferralCode { get; private set; }
    public string? Notes { get; private set; }

    // Reward
    public RewardStatus RewardStatus { get; private set; }
    public decimal? RewardAmount { get; private set; }
    public string? RewardType { get; private set; }
    public DateTime? RewardPaidAt { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ContactedAt { get; private set; }
    public DateTime? ConvertedAt { get; private set; }

    private CustomerReferral() { }

    public CustomerReferral(
        Guid tenantId,
        Guid referringCustomerId,
        string referredEmail,
        string? referredPhone,
        string? referredFirstName,
        string? referredLastName,
        string? referralSource,
        string? referralCode,
        string? notes)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (referringCustomerId == Guid.Empty)
            throw new ArgumentException("Referring customer ID is required", nameof(referringCustomerId));
        if (string.IsNullOrWhiteSpace(referredEmail))
            throw new ArgumentException("Referred email is required", nameof(referredEmail));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        ReferringCustomerId = referringCustomerId;
        ReferredEmail = referredEmail.Trim().ToLowerInvariant();
        ReferredPhone = referredPhone?.Trim();
        ReferredFirstName = referredFirstName?.Trim();
        ReferredLastName = referredLastName?.Trim();
        ReferralSource = referralSource?.Trim();
        ReferralCode = referralCode?.Trim();
        Notes = notes?.Trim();
        Status = CustomerReferralStatus.Pending;
        RewardStatus = RewardStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkContacted()
    {
        if (Status != CustomerReferralStatus.Pending)
            return;

        Status = CustomerReferralStatus.Contacted;
        ContactedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Convert(Guid newCustomerId, decimal? rewardAmount, string? rewardType)
    {
        if (Status == CustomerReferralStatus.Converted)
            return;

        Status = CustomerReferralStatus.Converted;
        ConvertedCustomerId = newCustomerId;
        ConvertedAt = DateTime.UtcNow;
        RewardAmount = rewardAmount;
        RewardType = rewardType;

        if (rewardAmount.HasValue && rewardAmount > 0)
        {
            RewardStatus = RewardStatus.Approved;
        }

        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status is CustomerReferralStatus.Converted or CustomerReferralStatus.Cancelled)
            return;

        Status = CustomerReferralStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Cancel()
    {
        if (Status == CustomerReferralStatus.Converted)
            return;

        Status = CustomerReferralStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkRewardPaid()
    {
        if (RewardStatus != RewardStatus.Approved)
            return;

        RewardStatus = RewardStatus.Paid;
        RewardPaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void CancelReward()
    {
        if (RewardStatus == RewardStatus.Paid)
            return;

        RewardStatus = RewardStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
}
