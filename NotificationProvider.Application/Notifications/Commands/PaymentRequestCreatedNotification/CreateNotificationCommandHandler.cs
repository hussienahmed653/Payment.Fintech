using ResultPattern.Abstraction;

namespace NotificationProvider.Application.Notifications.Commands.PaymentRequestCreatedNotification;

public class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand, Result>
{
    public Task<Result> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
