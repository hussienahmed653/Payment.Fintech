using NotificationProvider.Application.Common.Interfaces.FactorySelector;

namespace NotificationProvider.Application.SendNotification;

public class SendNotificationCommandHandler(INotificationProvider notificationProvider) : IRequestHandler<SendNotificationCommand, Result<object>>
{
    private readonly INotificationProvider _notificationProvider = notificationProvider;

    public async Task<Result<object>> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        var provider = _notificationProvider.Send((int)request.notificationRequest.Channel);
        return await provider.MakeNotification(request.notificationRequest);
    }
}
