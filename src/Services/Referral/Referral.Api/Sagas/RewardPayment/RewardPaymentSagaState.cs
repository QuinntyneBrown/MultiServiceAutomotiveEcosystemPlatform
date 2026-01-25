using MassTransit;

namespace Referral.Api.Sagas.RewardPayment;

public class RewardPaymentSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;

    // Referral information
    public Guid ReferralId { get; set; }
    public Guid TenantId { get; set; }
    public Guid RecipientId { get; set; }
    public string RecipientType { get; set; } = string.Empty; // Customer or Professional

    // Payment information
    public decimal Amount { get; set; }
    public string RewardType { get; set; } = string.Empty;
    public string? PaymentMethod { get; set; }
    public string? PaymentReference { get; set; }

    // Tracking
    public DateTime ApprovedAt { get; set; }
    public DateTime? ProcessingStartedAt { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? FailureReason { get; set; }
    public int RetryCount { get; set; }

    // Version for optimistic concurrency
    public int Version { get; set; }
}
