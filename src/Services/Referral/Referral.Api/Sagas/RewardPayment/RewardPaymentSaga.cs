using MassTransit;
using Shared.Contracts.Events.Referral;

namespace Referral.Api.Sagas.RewardPayment;

public class RewardPaymentSaga : MassTransitStateMachine<RewardPaymentSagaState>
{
    private const int MaxRetries = 3;

    public RewardPaymentSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => RewardApproved, x => x.CorrelateById(context => context.Message.ReferralId));
        Event(() => PaymentProcessingStarted, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentValidated, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentExecuted, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentFailed, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => PaymentRetryRequested, x => x.CorrelateById(context => context.Message.CorrelationId));

        Initially(
            When(RewardApproved)
                .Then(context =>
                {
                    context.Saga.ReferralId = context.Message.ReferralId;
                    context.Saga.TenantId = context.Message.TenantId;
                    context.Saga.RecipientId = context.Message.RecipientId;
                    context.Saga.RecipientType = context.Message.RecipientType;
                    context.Saga.Amount = context.Message.Amount;
                    context.Saga.RewardType = context.Message.RewardType;
                    context.Saga.ApprovedAt = context.Message.ApprovedAt;
                })
                .TransitionTo(ProcessingPayment)
                .Publish(context => new PaymentProcessingStarted(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    context.Saga.RecipientId,
                    context.Saga.Amount,
                    "Direct", // Default payment method
                    DateTime.UtcNow
                ))
        );

        During(ProcessingPayment,
            When(PaymentProcessingStarted)
                .Then(context =>
                {
                    context.Saga.ProcessingStartedAt = context.Message.StartedAt;
                    context.Saga.PaymentMethod = context.Message.PaymentMethod;
                })
                .TransitionTo(ValidatingPayment)
                .Publish(context => new PaymentValidated(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    true, // Simplified validation
                    null
                ))
        );

        During(ValidatingPayment,
            When(PaymentValidated)
                .IfElse(context => context.Message.IsValid,
                    valid => valid
                        .TransitionTo(ExecutingPayment)
                        .Publish(context => new PaymentExecuted(
                            context.Saga.CorrelationId,
                            context.Saga.ReferralId,
                            $"PAY-{context.Saga.ReferralId:N}",
                            DateTime.UtcNow
                        )),
                    invalid => invalid
                        .Then(context => context.Saga.FailureReason = context.Message.ValidationMessage)
                        .TransitionTo(Failed)
                        .Publish(context => new PaymentFailed(
                            context.Saga.CorrelationId,
                            context.Saga.ReferralId,
                            context.Saga.FailureReason ?? "Validation failed",
                            false,
                            DateTime.UtcNow
                        ))
                )
        );

        During(ExecutingPayment,
            When(PaymentExecuted)
                .Then(context =>
                {
                    context.Saga.PaymentReference = context.Message.PaymentReference;
                    context.Saga.PaidAt = context.Message.ExecutedAt;
                })
                .TransitionTo(Completed)
                .Publish(context => new RewardPaid(
                    context.Saga.ReferralId,
                    context.Saga.TenantId,
                    context.Saga.RecipientId,
                    context.Saga.Amount,
                    context.Saga.PaidAt ?? DateTime.UtcNow
                ))
                .Publish(context => new RewardPaymentCompleted(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    context.Saga.RecipientId,
                    context.Saga.Amount,
                    context.Saga.PaymentReference ?? string.Empty,
                    context.Saga.PaidAt ?? DateTime.UtcNow
                ))
                .Finalize(),
            When(PaymentFailed)
                .IfElse(context => context.Message.CanRetry && context.Saga.RetryCount < MaxRetries,
                    canRetry => canRetry
                        .Then(context => context.Saga.RetryCount++)
                        .TransitionTo(WaitingForRetry)
                        .Publish(context => new PaymentRetryRequested(
                            context.Saga.CorrelationId,
                            context.Saga.ReferralId,
                            context.Saga.RetryCount,
                            DateTime.UtcNow
                        )),
                    noRetry => noRetry
                        .Then(context => context.Saga.FailureReason = context.Message.Reason)
                        .TransitionTo(Failed)
                )
        );

        During(WaitingForRetry,
            When(PaymentRetryRequested)
                .TransitionTo(ProcessingPayment)
                .Publish(context => new PaymentProcessingStarted(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    context.Saga.RecipientId,
                    context.Saga.Amount,
                    context.Saga.PaymentMethod ?? "Direct",
                    DateTime.UtcNow
                ))
        );

        During(Failed,
            Ignore(PaymentProcessingStarted),
            Ignore(PaymentValidated),
            Ignore(PaymentExecuted),
            Ignore(PaymentFailed)
        );

        SetCompletedWhenFinalized();
    }

    // States
    public State ProcessingPayment { get; private set; } = null!;
    public State ValidatingPayment { get; private set; } = null!;
    public State ExecutingPayment { get; private set; } = null!;
    public State WaitingForRetry { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    // Events
    public Event<RewardApproved> RewardApproved { get; private set; } = null!;
    public Event<PaymentProcessingStarted> PaymentProcessingStarted { get; private set; } = null!;
    public Event<PaymentValidated> PaymentValidated { get; private set; } = null!;
    public Event<PaymentExecuted> PaymentExecuted { get; private set; } = null!;
    public Event<PaymentFailed> PaymentFailed { get; private set; } = null!;
    public Event<PaymentRetryRequested> PaymentRetryRequested { get; private set; } = null!;
}
