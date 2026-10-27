using Steeltoe.Discovery.Consul;

namespace Merchant.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddConsulConfiguration();
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
    public static IServiceCollection AddConsulConfiguration(this IServiceCollection services)
    {
        services.AddConsulDiscoveryClient();
        return services;
    }
}
