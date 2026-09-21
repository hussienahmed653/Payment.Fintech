using StackExchange.Redis;

namespace PaymentRequest.Application;

public static class DependancyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatRService()
            .AddFluentValidationService()
            .AddMapsterService()
            .AddRedisService(configuration);
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
    private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("Redis");
            options.InstanceName = "PaymentRequest_";
        });
        services.AddSingleton<IConnectionMultiplexer>(sp =>
        {
            var configurationOptions = configuration.GetConnectionString("Redis");
            return ConnectionMultiplexer.Connect(configurationOptions!);
        });
        return services;
    }
}
