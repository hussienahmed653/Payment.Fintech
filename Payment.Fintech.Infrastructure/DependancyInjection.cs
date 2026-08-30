namespace Payment.Fintech.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMerchantRepository, MerchantRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
