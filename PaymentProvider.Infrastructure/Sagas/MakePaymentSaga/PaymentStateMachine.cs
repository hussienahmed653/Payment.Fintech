using MassTransit;
using PaymentProvider.Application.Deposit.Command;
using Shared.Events.Payments;
using Shared.Messaging;

namespace PaymentProvider.Infrastructure.Sagas.MakePaymentSaga;

internal class PaymentStateMachine : MassTransitStateMachine<PaymentState>
{
    public PaymentStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => PaymentRequestCreated, x => x.CorrelateById(m => m.Message.CorrelationId));
        Event(() => PaymentRequestSucceeded, x => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(PaymentRequestCreated)
                .ThenAsync(InitializePaymentCreatedState)
                .TransitionTo(PaymentRequestSucceededState)
                .Publish(InitializeInitiateDepositCommand));

        During(PaymentRequestSucceededState,
            When(PaymentRequestSucceeded)
                .ThenAsync(InitializePaymentSucceededState)
                .TransitionTo(Completed).Finalize());
    }
    public State PaymentRequestCreatedState { get; private set; }
    public State PaymentRequestSucceededState { get; private set; }
    public State Completed { get; private set; }
    public Event<PaymentRequestCreatedIntegrationEvent> PaymentRequestCreated { get; private set; }
    public Event<PaymentRequestSucceededIntegrationEvent> PaymentRequestSucceeded { get; private set; }
    private static async Task InitializePaymentCreatedState(BehaviorContext<PaymentState, PaymentRequestCreatedIntegrationEvent> context)
    {
        context.Saga.InitiatedAt = DateTime.UtcNow;
        context.Saga.CorrelationId = context.Message.CorrelationId;
        context.Saga.CollectionMethod = context.Message.CollectionMethod ?? "";
    }
    private static async Task InitializePaymentSucceededState(BehaviorContext<PaymentState, PaymentRequestSucceededIntegrationEvent> context)
    {
        context.Saga.PaymentStatusChangedAt = DateTime.UtcNow;
    }
    //Uses Implicit operator to convert InitiateDepositMessage to InitiateDepositCommand
    private static InitiateDepositCommand InitializeInitiateDepositCommand(BehaviorContext<PaymentState, PaymentRequestCreatedIntegrationEvent> context)
    {
        var message = context.Message;
        return new InitiateDepositCommand
        {
            request = new Shared.PaymentEnvelope
            {
                payment = new Shared.Sections.PaymentRequestSection(1,
                                                                    message.MerchantId,
                                                                    2,
                                                                    message.CustomerId,
                                                                    message.Amount,
                                                                    Currency.AUD),
            }
        };
    }
}
