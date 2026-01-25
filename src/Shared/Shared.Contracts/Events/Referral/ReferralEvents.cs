namespace Shared.Contracts.Events.Referral;

public record CustomerReferralCreated(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferringCustomerId,
    string ReferredEmail,
    string? ReferredFirstName,
    string? ReferredLastName,
    string? ReferralCode,
    DateTime CreatedAt
) : IntegrationEvent;

public record CustomerReferralStatusChanged(
    Guid ReferralId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
) : IntegrationEvent;

public record CustomerReferralConverted(
    Guid ReferralId,
    Guid TenantId,
    Guid NewCustomerId,
    decimal? RewardAmount,
    DateTime ConvertedAt
) : IntegrationEvent;

public record ProfessionalReferralCreated(
    Guid ReferralId,
    Guid TenantId,
    Guid ReferringProfessionalId,
    string ReferredBusinessName,
    string ReferredEmail,
    DateTime CreatedAt
) : IntegrationEvent;

public record ProfessionalReferralStatusChanged(
    Guid ReferralId,
    Guid TenantId,
    string PreviousStatus,
    string NewStatus,
    DateTime ChangedAt
) : IntegrationEvent;

public record ProfessionalReferralConverted(
    Guid ReferralId,
    Guid TenantId,
    Guid ConvertedProfessionalId,
    decimal? RewardAmount,
    DateTime ConvertedAt
) : IntegrationEvent;

public record ReferralCodeCreated(
    Guid ReferralCodeId,
    Guid TenantId,
    string Code,
    Guid? CustomerId,
    Guid? ProfessionalId,
    DateTime CreatedAt
) : IntegrationEvent;

public record ReferralCodeUsed(
    Guid ReferralCodeId,
    Guid TenantId,
    string Code,
    Guid UsedById,
    DateTime UsedAt
) : IntegrationEvent;

public record RewardApproved(
    Guid ReferralId,
    Guid TenantId,
    Guid RecipientId,
    string RecipientType,
    decimal Amount,
    string RewardType,
    DateTime ApprovedAt
) : IntegrationEvent;

public record RewardPaid(
    Guid ReferralId,
    Guid TenantId,
    Guid RecipientId,
    decimal Amount,
    DateTime PaidAt
) : IntegrationEvent;
