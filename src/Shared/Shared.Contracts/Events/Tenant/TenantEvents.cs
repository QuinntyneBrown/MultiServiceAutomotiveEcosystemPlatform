namespace Shared.Contracts.Events.Tenant;

public record TenantCreated(
    Guid TenantId,
    string Name,
    string Slug,
    string? DisplayName,
    DateTime CreatedAt
) : IntegrationEvent;

public record TenantUpdated(
    Guid TenantId,
    string Name,
    string? DisplayName,
    string? LogoUrl,
    string? PrimaryColor,
    DateTime UpdatedAt
) : IntegrationEvent;

public record TenantStatusChanged(
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
) : IntegrationEvent;

public record TenantDeleted(
    Guid TenantId,
    DateTime DeletedAt
) : IntegrationEvent;
