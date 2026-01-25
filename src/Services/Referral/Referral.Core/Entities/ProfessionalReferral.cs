using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ProfessionalReferral : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ReferringProfessionalId { get; private set; }
    public string ReferredBusinessName { get; private set; } = string.Empty;
    public string ReferredEmail { get; private set; } = string.Empty;
    public string? ReferredPhone { get; private set; }
    public string? ContactPersonName { get; private set; }
    public Guid? ConvertedProfessionalId { get; private set; }
    public ProfessionalReferralStatus Status { get; private set; }
    public string? ReferralSource { get; private set; }
    public string? ReferralCode { get; private set; }
    public string? Notes { get; private set; }

    // Rewards
    public RewardStatus RewardStatus { get; private set; }
    public decimal? RewardAmount { get; private set; }
    public string? RewardType { get; private set; }
    public DateTime? RewardPaidAt { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? ContactedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? ConvertedAt { get; private set; }

    private ProfessionalReferral() { }

    public static ProfessionalReferral Create(
        Guid tenantId,
        Guid referringProfessionalId,
        string referredBusinessName,
        string referredEmail,
        string? referredPhone,
        string? contactPersonName,
        string? referralSource,
        string? referralCode,
        string? notes)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (referringProfessionalId == Guid.Empty)
            throw new ArgumentException("Referring professional ID is required", nameof(referringProfessionalId));
        if (string.IsNullOrWhiteSpace(referredBusinessName))
            throw new ArgumentException("Referred business name is required", nameof(referredBusinessName));
        if (string.IsNullOrWhiteSpace(referredEmail))
            throw new ArgumentException("Referred email is required", nameof(referredEmail));

        return new ProfessionalReferral
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            ReferringProfessionalId = referringProfessionalId,
            ReferredBusinessName = referredBusinessName.Trim(),
            ReferredEmail = referredEmail.Trim().ToLowerInvariant(),
            ReferredPhone = referredPhone?.Trim(),
            ContactPersonName = contactPersonName?.Trim(),
            ReferralSource = referralSource?.Trim(),
            ReferralCode = referralCode?.Trim(),
            Notes = notes?.Trim(),
            Status = ProfessionalReferralStatus.Pending,
            RewardStatus = RewardStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Accept()
    {
        if (Status != ProfessionalReferralStatus.Pending)
            return;

        Status = ProfessionalReferralStatus.Accepted;
        AcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Decline(string? reason = null)
    {
        if (Status != ProfessionalReferralStatus.Pending)
            return;

        Status = ProfessionalReferralStatus.Declined;
        if (!string.IsNullOrWhiteSpace(reason))
        {
            Notes = string.IsNullOrWhiteSpace(Notes)
                ? $"Declined: {reason}"
                : $"{Notes}\nDeclined: {reason}";
        }
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkContacted()
    {
        ContactedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Convert(Guid professionalId)
    {
        if (professionalId == Guid.Empty)
            throw new ArgumentException("Professional ID is required", nameof(professionalId));

        ConvertedProfessionalId = professionalId;
        Status = ProfessionalReferralStatus.Completed;
        ConvertedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status is ProfessionalReferralStatus.Completed or ProfessionalReferralStatus.Declined)
            return;

        Status = ProfessionalReferralStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ApproveReward(decimal amount, string rewardType)
    {
        RewardAmount = amount;
        RewardType = rewardType;
        RewardStatus = RewardStatus.Approved;
        UpdatedAt = DateTime.UtcNow;
    }

    public void PayReward()
    {
        if (RewardStatus != RewardStatus.Approved)
            return;

        RewardStatus = RewardStatus.Paid;
        RewardPaidAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}
