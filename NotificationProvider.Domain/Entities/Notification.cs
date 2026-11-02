using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid EventId { get; set; }
    public string TemplateKey { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<NotificationDelivery> Deliveries { get; set; } = [];
}
