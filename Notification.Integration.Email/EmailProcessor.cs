using NotificationProvider.Application.Common.Interfaces.Processor;
using ResultPattern.Abstraction;

namespace Notification.Integration.Email;

public class EmailProcessor() : INotificationProcessor
{
    public Task<Result<object>> MakeNotification(object request)
    {
        throw new NotImplementedException();
    }
}
