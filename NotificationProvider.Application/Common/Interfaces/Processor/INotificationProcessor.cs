namespace NotificationProvider.Application.Common.Interfaces.Processor;

public interface INotificationProcessor
{
    Task<Result<object>> MakeNotification(NotificationRequestSection request);
}
