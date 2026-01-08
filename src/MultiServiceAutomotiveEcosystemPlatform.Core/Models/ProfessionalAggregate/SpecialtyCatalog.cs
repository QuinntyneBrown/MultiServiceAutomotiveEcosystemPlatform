namespace MultiServiceAutomotiveEcosystemPlatform.Core.Models.ProfessionalAggregate;

public class SpecialtyCatalog
{
    public Guid SpecialtyCatalogId { get; private set; }
    public Guid? TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Slug { get; private set; } = string.Empty;
    public string Category { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Icon { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private SpecialtyCatalog() { }

    public SpecialtyCatalog(
        string name,
        string category,
        Guid? tenantId = null,
        string? description = null,
        string? icon = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.", nameof(category));

        SpecialtyCatalogId = Guid.NewGuid();
        TenantId = tenantId;
        Name = name.Trim();
        Slug = GenerateSlug(name);
        Category = category.Trim();
        Description = description?.Trim();
        Icon = icon?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateDetails(string name, string category, string? description, string? icon)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));
        if (string.IsNullOrWhiteSpace(category))
            throw new ArgumentException("Category cannot be empty.", nameof(category));

        Name = name.Trim();
        Category = category.Trim();
        Description = description?.Trim();
        Icon = icon?.Trim();
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public bool IsGlobal => TenantId == null;

    private static string GenerateSlug(string name)
    {
        var slug = name.ToLowerInvariant().Trim();
        slug = string.Join("-", slug.Split(new[] { ' ', '_', '-' }, StringSplitOptions.RemoveEmptyEntries));
        slug = new string(slug.Where(c => char.IsLetterOrDigit(c) || c == '-').ToArray());
        return slug;
    }
}
