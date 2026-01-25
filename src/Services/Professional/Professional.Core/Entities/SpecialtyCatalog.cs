using Shared.Contracts.Interfaces;

namespace Professional.Core.Entities;

public class SpecialtyCatalog : IEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string? Category { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private SpecialtyCatalog() { }

    public SpecialtyCatalog(string name, string? description, string? category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Id = Guid.NewGuid();
        Name = name.Trim();
        Description = description?.Trim();
        Category = category?.Trim();
        IsActive = true;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string? description, string? category)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required", nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        Category = category?.Trim();
        UpdatedAt = DateTime.UtcNow;
    }

    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
