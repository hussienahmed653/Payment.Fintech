using ResultPattern.Abstraction;
using Shared.Events.Payments;

namespace NotificationProvider.Application.Notifications.Commands.PaymentRequestCreatedNotification;

public record CreateNotificationCommand(PaymentRequestCreatedIntegrationEvent PaymentRequestCreatedEvent) : IRequest<Result>;
