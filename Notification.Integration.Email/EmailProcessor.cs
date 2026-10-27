using NotificationProvider.Application.Common.Interfaces.Processor;
using ResultPattern.Abstraction;
using Shared.Sections;

namespace Notification.Integration.Email;

public class EmailProcessor() : INotificationProcessor
{
    public Task<Result<object>> MakeNotification(NotificationRequestSection request)
    {
        throw new NotImplementedException();
    }
}
