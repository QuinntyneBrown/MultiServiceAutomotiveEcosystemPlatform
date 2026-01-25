using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ReferralCode : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public ReferralCodeType Type { get; private set; }
    public Guid OwnerId { get; private set; }
    public int MaxUses { get; private set; }
    public int CurrentUses { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }

    // Rewards
    public decimal? RewardAmount { get; private set; }
    public string? RewardType { get; private set; }
    public decimal? DiscountPercentage { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ReferralCode() { }

    public ReferralCode(
        Guid tenantId,
        string code,
        ReferralCodeType type,
        Guid ownerId,
        int maxUses,
        DateTime? expiresAt,
        decimal? rewardAmount,
        string? rewardType,
        decimal? discountPercentage)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required", nameof(code));
        if (ownerId == Guid.Empty)
            throw new ArgumentException("Owner ID is required", nameof(ownerId));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        Code = code.Trim().ToUpperInvariant();
        Type = type;
        OwnerId = ownerId;
        MaxUses = maxUses;
        ExpiresAt = expiresAt;
        RewardAmount = rewardAmount;
        RewardType = rewardType?.Trim();
        DiscountPercentage = discountPercentage;
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public bool IsValid()
    {
        if (!IsActive) return false;
        if (ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow) return false;
        if (MaxUses > 0 && CurrentUses >= MaxUses) return false;
        return true;
    }

    public void RecordUsage()
    {
        if (!IsValid())
            throw new InvalidOperationException("Code is not valid for use");

        CurrentUses++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
