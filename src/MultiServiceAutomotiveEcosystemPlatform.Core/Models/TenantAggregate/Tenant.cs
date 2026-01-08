using MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate.Enums;

namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.TenantAggregate;

public class Tenant
{
    public Guid TenantId { get; private set; }
    public string Slug { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string DisplayName { get; private set; } = string.Empty;
    public string? LogoUrl { get; private set; }
    public string? PrimaryColor { get; private set; }
    public string? Configuration { get; private set; }
    public TenantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Tenant() { }

    public Tenant(
        string slug,
        string name,
        string displayName,
        string? logoUrl = null,
        string? primaryColor = null,
        string? configuration = null)
    {
        if (string.IsNullOrWhiteSpace(slug))
            throw new ArgumentException("Slug cannot be empty.", nameof(slug));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty.", nameof(displayName));

        TenantId = Guid.NewGuid();
        Slug = slug.ToLowerInvariant().Trim();
        Name = name.Trim();
        DisplayName = displayName.Trim();
        LogoUrl = logoUrl;
        PrimaryColor = primaryColor;
        Configuration = configuration;
        Status = TenantStatus.Active;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string displayName, string? logoUrl, string? primaryColor)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Display name cannot be empty.", nameof(displayName));

        Name = name.Trim();
        DisplayName = displayName.Trim();
        LogoUrl = logoUrl;
        PrimaryColor = primaryColor;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateConfiguration(string? configuration)
    {
        Configuration = configuration;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = TenantStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool IsActive => Status == TenantStatus.Active;
}
