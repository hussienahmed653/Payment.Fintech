using Mapster;

namespace NotificationProvider.Infrastructure.Messaging.Consumers;

public record PaymentRequestSucceededConsumer(ISender sender) : IConsumer<PaymentRequestSucceededIntegrationEvent>
{
    private readonly ISender _sender = sender;
    public async Task Consume(ConsumeContext<PaymentRequestSucceededIntegrationEvent> context)
    {
        var message = context.Message.Adapt<PaymentRequestCreatedIntegrationEvent>();
        await _sender.Send(new CreateNotificationCommand(message));
    }
}
