namespace NotificationProvider.Infrastructure.Notifications;

internal class NotificationeDeliveryConfigration : IEntityTypeConfiguration<NotificationDelivery>
{
    public void Configure(EntityTypeBuilder<NotificationDelivery> builder)
    {
        builder.HasIndex(nd => new { nd.NotificationId, nd.Channel })
               .IsUnique();
    }
}
