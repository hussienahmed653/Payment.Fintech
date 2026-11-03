namespace NotificationProvider.Infrastructure.Messaging.Consumers;

public class PaymentRequestCreatedConsumer(ISender sender) : IConsumer<PaymentRequestCreatedIntegrationEvent>
{
    private readonly ISender _sender = sender;
    public async Task Consume(ConsumeContext<PaymentRequestCreatedIntegrationEvent> context) =>
        await _sender.Send(new CreateNotificationCommand(context.Message));
}
