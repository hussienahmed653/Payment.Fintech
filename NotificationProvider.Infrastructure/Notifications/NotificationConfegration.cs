namespace NotificationProvider.Infrastructure.Notifications;

internal class NotificationConfegration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasIndex(n => n.EventId)
            .IsUnique();
    }
}
