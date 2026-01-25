using Shared.Contracts.Enums;

namespace Shared.Contracts.DTOs;

public record TenantDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Slug { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string? LogoUrl { get; init; }
    public string? PrimaryColor { get; init; }
    public TenantStatus Status { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
}

public record CreateTenantRequest
{
    public string Name { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string? LogoUrl { get; init; }
    public string? PrimaryColor { get; init; }
}

public record UpdateTenantRequest
{
    public string Name { get; init; } = string.Empty;
    public string? DisplayName { get; init; }
    public string? LogoUrl { get; init; }
    public string? PrimaryColor { get; init; }
}

public record UpdateTenantStatusRequest
{
    public TenantStatus Status { get; init; }
}
