using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messaging;

public static class RabbitMqRegistration
{
    public static IServiceCollection AddRabbitMqMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator>? configureBus = null,
        Action<IBusRegistrationContext, IRabbitMqBusFactoryConfigurator>? configureEndpoints = null)
    {
        var options = configuration.GetSection(RabbitMqOptions.SectionName).Get<RabbitMqOptions>()
                      ?? new RabbitMqOptions();

        services.AddMassTransit(x =>
        {
            configureBus?.Invoke(x);

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(options.Host, options.VirtualHost, h =>
                {
                    h.Username(options.Username);
                    h.Password(options.Password);
                });

                configureEndpoints?.Invoke(context, cfg);
            });
        });

        return services;
    }
}
