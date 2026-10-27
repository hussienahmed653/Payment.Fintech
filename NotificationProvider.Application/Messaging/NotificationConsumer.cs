namespace NotificationProvider.Application.Messaging;

public class NotificationConsumer(ISender sender) : IConsumer<SendNotificationMessage>
{
    private readonly ISender _sender = sender;

    public async Task Consume(ConsumeContext<SendNotificationMessage> context)
    {
        var result = await _sender.Send(new SendNotificationCommand(context.Message.NotificationRequest), context.CancellationToken);
    }
}
