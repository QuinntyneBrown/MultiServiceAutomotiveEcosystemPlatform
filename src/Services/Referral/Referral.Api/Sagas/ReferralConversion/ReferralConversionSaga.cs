using MassTransit;
using Shared.Contracts.Events.Referral;

namespace Referral.Api.Sagas.ReferralConversion;

public class ReferralConversionSaga : MassTransitStateMachine<ReferralConversionSagaState>
{
    public ReferralConversionSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => ReferralConverted, x => x.CorrelateById(context => context.Message.ReferralId));
        Event(() => CodeValidated, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => CustomerCreated, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => CustomerCreationFailed, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => RewardCalculated, x => x.CorrelateById(context => context.Message.CorrelationId));
        Event(() => RewardApproved, x => x.CorrelateById(context => context.Message.ReferralId));

        Initially(
            When(ReferralConverted)
                .Then(context =>
                {
                    context.Saga.ReferralId = context.Message.ReferralId;
                    context.Saga.TenantId = context.Message.TenantId;
                    context.Saga.NewCustomerId = context.Message.NewCustomerId;
                    context.Saga.RewardAmount = context.Message.RewardAmount;
                    context.Saga.StartedAt = DateTime.UtcNow;
                })
                .TransitionTo(ValidatingCode)
                .Publish(context => new ReferralCodeValidated(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    context.Saga.TenantId,
                    true, // Validation happens synchronously in this simplified version
                    null
                ))
        );

        During(ValidatingCode,
            When(CodeValidated)
                .IfElse(context => context.Message.IsValid,
                    valid => valid
                        .TransitionTo(CalculatingReward)
                        .Publish(context => new RewardCalculated(
                            context.Saga.CorrelationId,
                            context.Saga.ReferralId,
                            context.Saga.RewardAmount ?? 0,
                            "ReferralBonus"
                        )),
                    invalid => invalid
                        .Then(context => context.Saga.FailureReason = context.Message.ValidationMessage)
                        .TransitionTo(Failed)
                        .Publish(context => new ReferralConversionFailed(
                            context.Saga.CorrelationId,
                            context.Saga.ReferralId,
                            context.Saga.FailureReason ?? "Code validation failed",
                            DateTime.UtcNow
                        ))
                )
        );

        During(CalculatingReward,
            When(RewardCalculated)
                .Then(context =>
                {
                    context.Saga.RewardAmount = context.Message.RewardAmount;
                    context.Saga.RewardType = context.Message.RewardType;
                })
                .TransitionTo(ApprovingReward)
                .Publish(context => new RewardApproved(
                    context.Saga.ReferralId,
                    context.Saga.TenantId,
                    context.Saga.ReferringCustomerId,
                    "Customer",
                    context.Saga.RewardAmount ?? 0,
                    context.Saga.RewardType ?? "ReferralBonus",
                    DateTime.UtcNow
                ))
        );

        During(ApprovingReward,
            When(RewardApproved)
                .Then(context => context.Saga.CompletedAt = DateTime.UtcNow)
                .TransitionTo(Completed)
                .Publish(context => new ReferralConversionCompleted(
                    context.Saga.CorrelationId,
                    context.Saga.ReferralId,
                    context.Saga.NewCustomerId,
                    context.Saga.RewardAmount,
                    context.Saga.CompletedAt ?? DateTime.UtcNow
                ))
                .Finalize()
        );

        During(Failed,
            Ignore(CodeValidated),
            Ignore(RewardCalculated),
            Ignore(RewardApproved)
        );

        SetCompletedWhenFinalized();
    }

    // States
    public State ValidatingCode { get; private set; } = null!;
    public State CalculatingReward { get; private set; } = null!;
    public State ApprovingReward { get; private set; } = null!;
    public State Completed { get; private set; } = null!;
    public State Failed { get; private set; } = null!;

    // Events
    public Event<CustomerReferralConverted> ReferralConverted { get; private set; } = null!;
    public Event<ReferralCodeValidated> CodeValidated { get; private set; } = null!;
    public Event<CustomerCreationCompleted> CustomerCreated { get; private set; } = null!;
    public Event<CustomerCreationFailed> CustomerCreationFailed { get; private set; } = null!;
    public Event<RewardCalculated> RewardCalculated { get; private set; } = null!;
    public Event<RewardApproved> RewardApproved { get; private set; } = null!;
}
