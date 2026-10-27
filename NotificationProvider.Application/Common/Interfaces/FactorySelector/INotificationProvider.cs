namespace NotificationProvider.Application.Common.Interfaces.FactorySelector;

public interface INotificationProvider
{
    INotificationProcessor Send(int channel);
}
