using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

public class ReferralCode
{
    public Guid ReferralCodeId { get; private set; }
    public Guid TenantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public ReferralCodeType CodeType { get; private set; }

    // Owner
    public Guid? CustomerId { get; private set; }
    public Guid? ProfessionalId { get; private set; }
    public Guid? CampaignId { get; private set; }

    // Configuration
    public int? MaxUses { get; private set; }
    public int CurrentUses { get; private set; }
    public decimal? RewardAmount { get; private set; }
    public decimal? DiscountPercentage { get; private set; }

    // Status
    public bool IsActive { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private ReferralCode() { }

    public ReferralCode(
        Guid tenantId,
        string code,
        ReferralCodeType codeType,
        Guid? customerId = null,
        Guid? professionalId = null,
        Guid? campaignId = null,
        int? maxUses = null,
        decimal? rewardAmount = null,
        decimal? discountPercentage = null,
        DateTime? expiresAt = null)
    {
        ValidateOwnership(codeType, customerId, professionalId, campaignId);

        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code cannot be empty.", nameof(code));

        ReferralCodeId = Guid.NewGuid();
        TenantId = tenantId;
        Code = code.ToUpperInvariant().Trim();
        CodeType = codeType;
        CustomerId = customerId;
        ProfessionalId = professionalId;
        CampaignId = campaignId;
        MaxUses = maxUses;
        CurrentUses = 0;
        RewardAmount = rewardAmount;
        DiscountPercentage = discountPercentage;
        IsActive = true;
        ExpiresAt = expiresAt;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementUses()
    {
        if (!CanBeUsed)
            throw new InvalidOperationException("This referral code cannot be used.");

        CurrentUses++;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateConfiguration(int? maxUses, decimal? rewardAmount, decimal? discountPercentage)
    {
        MaxUses = maxUses;
        RewardAmount = rewardAmount;
        DiscountPercentage = discountPercentage;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetExpiration(DateTime? expiresAt)
    {
        ExpiresAt = expiresAt;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        IsActive = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        IsActive = false;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value;
    public bool HasReachedMaxUses => MaxUses.HasValue && CurrentUses >= MaxUses.Value;
    public bool CanBeUsed => IsActive && !IsExpired && !HasReachedMaxUses;
    public int? RemainingUses => MaxUses.HasValue ? MaxUses.Value - CurrentUses : null;

    private static void ValidateOwnership(
        ReferralCodeType codeType,
        Guid? customerId,
        Guid? professionalId,
        Guid? campaignId)
    {
        switch (codeType)
        {
            case ReferralCodeType.Customer when !customerId.HasValue:
                throw new ArgumentException("Customer ID is required for customer referral codes.");
            case ReferralCodeType.Professional when !professionalId.HasValue:
                throw new ArgumentException("Professional ID is required for professional referral codes.");
            case ReferralCodeType.Campaign when !campaignId.HasValue:
                throw new ArgumentException("Campaign ID is required for campaign referral codes.");
        }
    }
}
