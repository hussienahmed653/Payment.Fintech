using NotificationProvider.Infrastructure.Messaging.Consumers;
using Shared.Messaging;

namespace NotificationProvider.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();
        services.AddRabbitMqMessaging(configuration, configureBus:
            x =>
            {
                x.AddConsumer<PaymentRequestCreatedConsumer>();
                x.AddConsumer<PaymentRequestSucceededConsumer>();
            },
            configureEndpoints: (context, cfg) =>
            {
                cfg.ReceiveEndpoint(QueueNames.CreatePaymentRequest, e =>
                {
                    e.ConfigureConsumer<PaymentRequestCreatedConsumer>(context);
                });
                cfg.ReceiveEndpoint(QueueNames.SuccessPaymentRequest, e =>
                {
                    e.ConfigureConsumer<PaymentRequestSucceededConsumer>(context);
                });
            });

        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        var databaseName = configuration["MongoDbSettings:DatabaseName"];
        services.AddSingleton<IMongoClient>(sp => new MongoClient(connectionString));
        services.AddScoped(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            return client.GetDatabase(databaseName);
        });
        services.AddScoped<ApplicationDbContext>();
        return services;
    }
}
