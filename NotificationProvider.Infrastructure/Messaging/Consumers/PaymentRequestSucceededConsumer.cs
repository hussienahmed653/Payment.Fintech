namespace NotificationProvider.Infrastructure.Messaging.Consumers;

public record PaymentRequestSucceededConsumer(ISender sender) : IConsumer<PaymentRequestSucceededIntegrationEvent>
{
    private readonly ISender _sender = sender;
    public async Task Consume(ConsumeContext<PaymentRequestSucceededIntegrationEvent> context)
    {
        await _sender.Send(new CreateNotificationCommand(context.Message));
    }
}
