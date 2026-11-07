using MassTransit;
using PaymentProvider.Application.Messaging;
using PaymentProvider.Infrastructure.Sagas;
using PaymentProvider.Infrastructure.Sagas.MakePaymentSaga;
using PaymentProvider.Integration.MMO;
using Shared.Messaging;
using Steeltoe.Discovery.Consul;

namespace PaymentProvider.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // تسجيل موحد لـ MassTransit يحوي الـ Consumer والـ Saga والـ Bus
        services.AddRabbitMqMessaging(configuration,
            configureBus: x =>
            {
                // 1. تسجيل الـ Consumer المعرف في الـ Application
                x.AddConsumer<InitiateDepositConsumer>();

                // 2. تسجيل الـ Saga State Machine المعرفة في الـ Infrastructure
                x.AddSagaStateMachine<PaymentStateMachine, PaymentState>()
                 .InMemoryRepository();
            },
            configureEndpoints: (context, cfg) =>
            {
                // 3. السطر ده هو اللي بينشئ الـ Queues والـ Bindings للـ Consumer وللـ Saga مع بعض
                cfg.ConfigureEndpoints(context);
            });

        // باقي خدمات الـ Infrastructure...
        services.AddConsulConfiguration();
        services.AddHttpClient<PawaPayProcessor>();
        services.AddScoped<ISelectorFactory, SelectorFactory>();
        services.AddKeyedScoped<IPaymentProcessor, PawaPayProcessor>((int)PaymentProviderType.PawaPay);
        services.AddKeyedScoped<IPaymentProcessor, MMOProcessor>((int)PaymentProviderType.MMO);

        return services;
    }
    public static IServiceCollection AddConsulConfiguration(this IServiceCollection services)
    {
        services.AddConsulDiscoveryClient();
        return services;
    }
}
