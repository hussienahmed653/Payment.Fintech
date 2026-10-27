using NotificationProvider.Domain.Enums;

namespace NotificationProvider.Infrastructure.NotificationProvider.Persistence;

internal class NotificationProvider(IServiceProvider serviceProvider) : INotificationProvider
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public INotificationProcessor Send(int channel)
    {
        return channel switch
        {
            (int)ChannelEnums.Email => _serviceProvider.GetRequiredKeyedService<INotificationProcessor>((int)ChannelEnums.Email),
            (int)ChannelEnums.SMS => _serviceProvider.GetRequiredKeyedService<INotificationProcessor>((int)ChannelEnums.SMS),
            _ => throw new ArgumentException($"Invalid channel: {channel}")
        };
    }
}
