using System.Text.RegularExpressions;
using Shared.Contracts.Enums;
using Shared.Contracts.Interfaces;

namespace Tenant.Core.Entities;

public partial class Tenant : IAggregateRoot
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string? DisplayName { get; private set; }
    public string? LogoUrl { get; private set; }
    public string? PrimaryColor { get; private set; }
    public TenantStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Tenant() { }

    public Tenant(string name, string? displayName = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Id = Guid.NewGuid();
        Name = name.Trim();
        Slug = GenerateSlug(name);
        DisplayName = displayName?.Trim();
        Status = TenantStatus.Active;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? displayName, string? logoUrl, string? primaryColor)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name.Trim();
        Slug = GenerateSlug(name);
        DisplayName = displayName?.Trim();
        LogoUrl = logoUrl?.Trim();
        PrimaryColor = primaryColor?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate()
    {
        if (Status == TenantStatus.Active)
            return;

        Status = TenantStatus.Active;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Suspend()
    {
        if (Status == TenantStatus.Suspended)
            return;

        Status = TenantStatus.Suspended;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == TenantStatus.Inactive)
            return;

        Status = TenantStatus.Inactive;
        UpdatedAt = DateTime.UtcNow;
    }

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant();
        slug = SlugRegex().Replace(slug, "-");
        slug = MultipleHyphensRegex().Replace(slug, "-");
        slug = slug.Trim('-');
        return slug;
    }

    [GeneratedRegex(@"[^a-z0-9\s-]")]
    private static partial Regex SlugRegex();

    [GeneratedRegex(@"-+")]
    private static partial Regex MultipleHyphensRegex();
}
