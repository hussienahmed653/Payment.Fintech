namespace PaymentRequest.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IPaymentRepository, PaymentRequestRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();
        services.AddScoped<IPaymentGatewayRepository, PaymentGatewayRepository>();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        return services;
    }
}
