using Customer.Application.Messaging;
using MassTransit;
using Shared.Messaging;

namespace Customer.Application;

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
                    x.AddConsumer<UpdateCustomerConsumer>();
                    x.AddRequestClient<CreatePaymentMessage>(new Uri($"queue:{QueueNames.CreatePayment}"));
                },
                configureEndpoints: (context, cfg) =>
                {
                    cfg.ReceiveEndpoint(QueueNames.UpdateCustomer, e =>
                    {
                        e.ConfigureConsumer<UpdateCustomerConsumer>(context);
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
