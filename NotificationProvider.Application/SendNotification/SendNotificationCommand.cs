using Shared.Sections;

namespace NotificationProvider.Application.SendNotification;

public record SendNotificationCommand(NotificationRequestSection notificationRequest) : IRequest<Result<object>>;
