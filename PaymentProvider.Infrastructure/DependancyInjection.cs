namespace PaymentProvider.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<PawaPayProcessor>();
        services.AddScoped<ISelectorFactory, SelectorFactory>();
        services.AddKeyedScoped<IPaymentProcessor, PawaPayProcessor>((int)PaymentProviderType.PawaPay);

        return services;
    }
}
