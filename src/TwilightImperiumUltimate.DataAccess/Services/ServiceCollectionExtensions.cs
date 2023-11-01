using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.DataAccess.Services;

public static class ServiceCollectionExtensions
{
    private const string DbConnectionStringName = "TwilightImperium";

    public static IServiceCollection RegisterDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<TwilightImperiumDbContext>(
            options =>
            options.UseSqlServer(configuration.GetConnectionString(DbConnectionStringName))
                   .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                   .EnableSensitiveDataLogging()
                   .EnableDetailedErrors(),
            contextLifetime: ServiceLifetime.Transient,
            optionsLifetime: ServiceLifetime.Singleton);

        services.AddDbContextFactory<TwilightImperiumDbContext>(
            options =>
            options.UseSqlServer(configuration.GetConnectionString(DbConnectionStringName)));

        services.AddScoped<ISystemTileRepository, SystemTileRepository>();

        services.AddSingleton<TwilightImperiumDbContextInitializer>();
        services.AddSingleton<ICreateCardImagePathService, CreateCardImagePathService>();

        return services;
    }
}
