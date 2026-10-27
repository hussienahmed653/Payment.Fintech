namespace NotificationProvider.Application.Common.Interfaces.Processor;

public interface INotificationProcessor
{
    Task<Result<object>> MakeNotification(object request);
}
