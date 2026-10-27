using PaymentProvider.Integration.MMO;
using Steeltoe.Discovery.Consul;

namespace PaymentProvider.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
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
