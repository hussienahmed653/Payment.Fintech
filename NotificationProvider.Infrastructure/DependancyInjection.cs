namespace NotificationProvider.Infrastructure;

public static class DependancyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUnitOfWork, UnitOfWorkRepository>();

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
