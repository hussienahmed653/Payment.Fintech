using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Application.Notifications.Policies;

public static class PaymentRequestCreatedPolicy
{
    public static readonly NotificationChannel[] CustomerChannel =
    [
        NotificationChannel.Email
    ];
    public static readonly NotificationChannel[] MerchantChannel =
    [
        NotificationChannel.Email,
        NotificationChannel.SMS
    ];
}
