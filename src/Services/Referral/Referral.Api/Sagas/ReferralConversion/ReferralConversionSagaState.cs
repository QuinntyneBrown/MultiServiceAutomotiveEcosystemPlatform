using MassTransit;

namespace Referral.Api.Sagas.ReferralConversion;

public class ReferralConversionSagaState : SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; } = string.Empty;

    // Referral information
    public Guid ReferralId { get; set; }
    public Guid TenantId { get; set; }
    public Guid ReferringCustomerId { get; set; }
    public Guid NewCustomerId { get; set; }
    public string? ReferralCode { get; set; }

    // Reward information
    public decimal? RewardAmount { get; set; }
    public string? RewardType { get; set; }

    // Tracking
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? FailureReason { get; set; }

    // Version for optimistic concurrency
    public int Version { get; set; }
}
