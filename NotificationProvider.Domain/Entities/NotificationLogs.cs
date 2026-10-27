using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Domain.Entities;

public class NotificationLogs
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public ChannelEnums Channel { get; set; }
    public string Recipient { get; set; }
    public string? Subject { get; set; }
    public string Body { get; set; }
    public GCNotificationStatus Status { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? SentAt { get; set; }
}
