using Shared.Contracts.Interfaces;

namespace Referral.Core.Entities;

public class ReferralCode : IAggregateRoot, ITenantEntity
{
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public Guid? CustomerId { get; private set; }
    public Guid? ProfessionalId { get; private set; }
    public string? CampaignName { get; private set; }
    public int? MaxUses { get; private set; }
    public int CurrentUses { get; private set; }
    public DateTime? ExpiresAt { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private ReferralCode() { }

    public static ReferralCode Create(
        Guid tenantId,
        string code,
        Guid? customerId,
        Guid? professionalId,
        string? campaignName,
        DateTime? expiresAt,
        int? maxUses)
    {
        if (tenantId == Guid.Empty)
            throw new ArgumentException("Tenant ID is required", nameof(tenantId));
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Code is required", nameof(code));
        if (!customerId.HasValue && !professionalId.HasValue)
            throw new ArgumentException("Either CustomerId or ProfessionalId is required");

        return new ReferralCode
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Code = code.Trim().ToUpperInvariant(),
            CustomerId = customerId,
            ProfessionalId = professionalId,
            CampaignName = campaignName?.Trim(),
            MaxUses = maxUses,
            ExpiresAt = expiresAt,
            IsActive = true,
            CurrentUses = 0,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool IsValid()
    {
        if (!IsActive) return false;
        if (ExpiresAt.HasValue && ExpiresAt.Value < DateTime.UtcNow) return false;
        if (MaxUses.HasValue && CurrentUses >= MaxUses.Value) return false;
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
