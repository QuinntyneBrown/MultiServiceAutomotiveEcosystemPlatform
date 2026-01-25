namespace Referral.Api.Sagas.RewardPayment;

// Saga-specific events for internal coordination

public record PaymentProcessingStarted(
    Guid CorrelationId,
    Guid ReferralId,
    Guid RecipientId,
    decimal Amount,
    string PaymentMethod,
    DateTime StartedAt
);

public record PaymentValidated(
    Guid CorrelationId,
    Guid ReferralId,
    bool IsValid,
    string? ValidationMessage
);

public record PaymentExecuted(
    Guid CorrelationId,
    Guid ReferralId,
    string PaymentReference,
    DateTime ExecutedAt
);

public record PaymentFailed(
    Guid CorrelationId,
    Guid ReferralId,
    string Reason,
    bool CanRetry,
    DateTime FailedAt
);

public record PaymentRetryRequested(
    Guid CorrelationId,
    Guid ReferralId,
    int RetryAttempt,
    DateTime RequestedAt
);

public record RewardPaymentCompleted(
    Guid CorrelationId,
    Guid ReferralId,
    Guid RecipientId,
    decimal Amount,
    string PaymentReference,
    DateTime CompletedAt
);
