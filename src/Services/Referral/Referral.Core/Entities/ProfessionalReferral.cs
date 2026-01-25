using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ProfessionalReferral : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public Guid ReferringProfessionalId { get; private set; }
    public Guid ReferredProfessionalId { get; private set; }
    public Guid CustomerId { get; private set; }
    public ProfessionalReferralStatus Status { get; private set; }
    public string? ServiceType { get; private set; }
    public string? Notes { get; private set; }

    // Discount
    public string? DiscountType { get; private set; }
    public decimal? DiscountValue { get; private set; }
    public string? DiscountCode { get; private set; }
    public bool DiscountUsed { get; private set; }

    // Timestamps
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? AcceptedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }
    public DateTime? DeclinedAt { get; private set; }

    private ProfessionalReferral() { }

    public ProfessionalReferral(
        Guid tenantId,
        Guid referringProfessionalId,
        Guid referredProfessionalId,
        Guid customerId,
        string? serviceType,
        string? notes,
        string? discountType,
        decimal? discountValue)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (referringProfessionalId == Guid.Empty)
            throw new ArgumentException("Referring professional ID is required", nameof(referringProfessionalId));
        if (referredProfessionalId == Guid.Empty)
            throw new ArgumentException("Referred professional ID is required", nameof(referredProfessionalId));
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID is required", nameof(customerId));

        Id = Guid.NewGuid();
        TenantId = tenantId;
        ReferringProfessionalId = referringProfessionalId;
        ReferredProfessionalId = referredProfessionalId;
        CustomerId = customerId;
        ServiceType = serviceType?.Trim();
        Notes = notes?.Trim();
        DiscountType = discountType?.Trim();
        DiscountValue = discountValue;
        Status = ProfessionalReferralStatus.Pending;
        CreatedAt = DateTime.UtcNow;
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
        Notes = string.IsNullOrEmpty(reason) ? Notes : $"{Notes}\nDeclined: {reason}".Trim();
        DeclinedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Complete()
    {
        if (Status != ProfessionalReferralStatus.Accepted)
            return;

        Status = ProfessionalReferralStatus.Completed;
        CompletedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (Status is ProfessionalReferralStatus.Completed or ProfessionalReferralStatus.Declined)
            return;

        Status = ProfessionalReferralStatus.Expired;
        UpdatedAt = DateTime.UtcNow;
    }

    public void SetDiscountCode(string code)
    {
        DiscountCode = code;
        UpdatedAt = DateTime.UtcNow;
    }

    public void MarkDiscountUsed()
    {
        DiscountUsed = true;
        UpdatedAt = DateTime.UtcNow;
    }
}
