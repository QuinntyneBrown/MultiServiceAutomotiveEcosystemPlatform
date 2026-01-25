namespace Referral.Api.Sagas.ReferralConversion;

// Saga-specific events for internal coordination

public record ReferralCodeValidated(
    Guid CorrelationId,
    Guid ReferralId,
    Guid TenantId,
    bool IsValid,
    string? ValidationMessage
);

public record CustomerCreationRequested(
    Guid CorrelationId,
    Guid ReferralId,
    Guid TenantId,
    string Email,
    string? FirstName,
    string? LastName,
    string? Phone
);

public record CustomerCreationCompleted(
    Guid CorrelationId,
    Guid ReferralId,
    Guid NewCustomerId
);

public record CustomerCreationFailed(
    Guid CorrelationId,
    Guid ReferralId,
    string Reason
);

public record RewardCalculated(
    Guid CorrelationId,
    Guid ReferralId,
    decimal RewardAmount,
    string RewardType
);

public record ReferralConversionCompleted(
    Guid CorrelationId,
    Guid ReferralId,
    Guid NewCustomerId,
    decimal? RewardAmount,
    DateTime CompletedAt
);

public record ReferralConversionFailed(
    Guid CorrelationId,
    Guid ReferralId,
    string Reason,
    DateTime FailedAt
);

public record ReferralCodeUsageRestored(
    Guid CorrelationId,
    Guid ReferralId,
    string Code
);
