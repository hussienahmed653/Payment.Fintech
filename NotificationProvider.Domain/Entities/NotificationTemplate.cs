using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Domain.Entities;

public class NotificationTemplate
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public ChannelEnums Channel { get; set; }
    public string SubjectTemplate { get; set; }
    public string BodyTemplate { get; set; }
}
