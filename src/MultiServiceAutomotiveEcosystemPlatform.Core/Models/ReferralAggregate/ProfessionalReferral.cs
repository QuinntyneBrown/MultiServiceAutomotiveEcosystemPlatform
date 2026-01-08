using MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ReferralAggregate;

public class ProfessionalReferral
{
    public Guid ProfessionalReferralId { get; private set; }
    public Guid TenantId { get; private set; }

    // Parties
    public Guid SourceProfessionalId { get; private set; }
    public Guid TargetProfessionalId { get; private set; }
    public Guid CustomerId { get; private set; }

    // Context
    public string? Reason { get; private set; }
    public string? ServiceNeeded { get; private set; }
    public string? Notes { get; private set; }
    public ReferralPriority Priority { get; private set; }

    // Status
    public ProfessionalReferralStatus Status { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public string? DeclinedReason { get; private set; }

    // Discount
    public bool DiscountOffered { get; private set; }
    public DiscountType DiscountType { get; private set; }
    public decimal? DiscountValue { get; private set; }
    public string? DiscountCode { get; private set; }
    public bool DiscountUsed { get; private set; }

    // Follow-up
    public bool FollowUpRequired { get; private set; }
    public DateTime? FollowUpDate { get; private set; }
    public string? FollowUpNotes { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public DateTime? ExpiresAt { get; private set; }

    private ProfessionalReferral() { }

    public ProfessionalReferral(
        Guid tenantId,
        Guid sourceProfessionalId,
        Guid targetProfessionalId,
        Guid customerId,
        string? reason = null,
        string? serviceNeeded = null,
        string? notes = null,
        ReferralPriority priority = ReferralPriority.Normal,
        int expirationDays = 30)
    {
        if (sourceProfessionalId == targetProfessionalId)
            throw new ArgumentException("Source and target professional cannot be the same.");

        ProfessionalReferralId = Guid.NewGuid();
        TenantId = tenantId;
        SourceProfessionalId = sourceProfessionalId;
        TargetProfessionalId = targetProfessionalId;
        CustomerId = customerId;
        Reason = reason?.Trim();
        ServiceNeeded = serviceNeeded?.Trim();
        Notes = notes?.Trim();
        Priority = priority;
        Status = ProfessionalReferralStatus.Pending;
        DiscountType = DiscountType.None;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ExpiresAt = DateTime.UtcNow.AddDays(expirationDays);
    }

    public void SetDiscount(DiscountType discountType, decimal? discountValue, string? discountCode)
    {
        if (discountType != DiscountType.None && !discountValue.HasValue)
            throw new ArgumentException("Discount value is required when discount type is set.");

        DiscountOffered = discountType != DiscountType.None;
        DiscountType = discountType;
        DiscountValue = discountValue;
        DiscountCode = discountCode?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Accept()
    {
        if (Status != ProfessionalReferralStatus.Pending)
            throw new InvalidOperationException("Can only accept pending referrals.");

        Status = ProfessionalReferralStatus.Accepted;
        AcceptedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Decline(string? reason = null)
    {
        if (Status != ProfessionalReferralStatus.Pending)
            throw new InvalidOperationException("Can only decline pending referrals.");

        Status = ProfessionalReferralStatus.Declined;
        DeclinedReason = reason?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != ProfessionalReferralStatus.Accepted)
            throw new InvalidOperationException("Can only complete accepted referrals.");

        Status = ProfessionalReferralStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status != ProfessionalReferralStatus.Pending)
            throw new InvalidOperationException("Can only expire pending referrals.");

        Status = ProfessionalReferralStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkDiscountUsed()
    {
        if (!DiscountOffered)
            throw new InvalidOperationException("No discount was offered for this referral.");

        DiscountUsed = true;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetFollowUp(DateTime followUpDate, string? notes = null)
    {
        FollowUpRequired = true;
        FollowUpDate = followUpDate;
        FollowUpNotes = notes?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void ClearFollowUp()
    {
        FollowUpRequired = false;
        FollowUpDate = null;
        FollowUpNotes = null;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateNotes(string? notes)
    {
        Notes = notes?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsExpired => ExpiresAt.HasValue && DateTime.UtcNow > ExpiresAt.Value && Status == ProfessionalReferralStatus.Pending;
    public bool IsCompleted => Status == ProfessionalReferralStatus.Completed;
    public bool IsPending => Status == ProfessionalReferralStatus.Pending;
    public bool IsActive => Status == ProfessionalReferralStatus.Pending || Status == ProfessionalReferralStatus.Accepted;
}
