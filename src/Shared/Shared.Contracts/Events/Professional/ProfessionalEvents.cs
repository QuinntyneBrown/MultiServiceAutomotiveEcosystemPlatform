namespace Shared.Contracts.Events.Professional;

public record ProfessionalCreated(
    Guid ProfessionalId,
    Guid TenantId,
    string BusinessName,
    string Email,
    string Slug,
    DateTime CreatedAt
) : IntegrationEvent;

public record ProfessionalUpdated(
    Guid ProfessionalId,
    Guid TenantId,
    string BusinessName,
    string Email,
    DateTime UpdatedAt
) : IntegrationEvent;

public record ProfessionalVerified(
    Guid ProfessionalId,
    Guid TenantId,
    DateTime VerifiedAt
) : IntegrationEvent;

public record ProfessionalStatusChanged(
    Guid ProfessionalId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
) : IntegrationEvent;

public record ProfessionalSpecialtyAdded(
    Guid ProfessionalId,
    Guid TenantId,
    Guid SpecialtyId,
    string SpecialtyName,
    DateTime AddedAt
) : IntegrationEvent;

public record ProfessionalSpecialtyRemoved(
    Guid ProfessionalId,
    Guid TenantId,
    Guid SpecialtyId,
    DateTime RemovedAt
) : IntegrationEvent;

public record ProfessionalDeleted(
    Guid ProfessionalId,
    Guid TenantId,
    DateTime DeletedAt
) : IntegrationEvent;
