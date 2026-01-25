namespace Shared.Contracts.Events.Customer;

public record CustomerCreated(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    DateTime CreatedAt
) : IntegrationEvent;

public record CustomerUpdated(
    Guid CustomerId,
    Guid TenantId,
    string Email,
    string FirstName,
    string LastName,
    string? Phone,
    DateTime UpdatedAt
) : IntegrationEvent;

public record CustomerOwnershipChanged(
    Guid CustomerId,
    Guid TenantId,
    Guid? PreviousOwnerId,
    Guid NewOwnerId,
    string Reason,
    DateTime ChangedAt
) : IntegrationEvent;

public record CustomerStatusChanged(
    Guid CustomerId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
) : IntegrationEvent;

public record CustomerVerified(
    Guid CustomerId,
    Guid TenantId,
    string VerificationType, // Email, Phone
    DateTime VerifiedAt
) : IntegrationEvent;

public record CustomerDeleted(
    Guid CustomerId,
    Guid TenantId,
    DateTime DeletedAt
) : IntegrationEvent;
