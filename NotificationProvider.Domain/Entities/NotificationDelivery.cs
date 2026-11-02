using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Domain.Entities;

public class NotificationDelivery
{
    public Guid Id { get; set; }
    public Guid NotificationId { get; set; }
    public Notification Notification { get; set; } = default!;
    public NotificationChannel Channel { get; set; }
    public string Recipient { get; set; } = null!;
    public DeliveryStatus Status { get; set; }
    public int Attempts { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? SentAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
