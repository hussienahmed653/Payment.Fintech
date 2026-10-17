using MassTransit;
using PaymentProvider.Application.Messaging;
using Shared.Messaging;
using ValidationPipeline.Behaviors;

namespace PaymentProvider.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatRService()
            .AddFluentValidationService()
            .AddMapsterService()
            .AddRabbitMqMessaging(
                configuration,
                configureBus: x =>
                {
                    x.AddConsumer<InitiateDepositConsumer>();
                },
                configureEndpoints: (context, cfg) =>
                {
                    cfg.ReceiveEndpoint(QueueNames.InitiateDeposit, e =>
                    {
                        e.ConfigureConsumer<InitiateDepositConsumer>(context);
                    });
                });
        return services;
    }
    private static IServiceCollection AddMediatRService(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
        }
        );
        return services;
    }
    private static IServiceCollection AddFluentValidationService(this IServiceCollection services)
    {
        services
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
            .AddFluentValidationAutoValidation();
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
    private static IServiceCollection AddMapsterService(this IServiceCollection services)
    {
        var mapconfig = TypeAdapterConfig.GlobalSettings;
        mapconfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mapconfig));
        return services;
    }
}
