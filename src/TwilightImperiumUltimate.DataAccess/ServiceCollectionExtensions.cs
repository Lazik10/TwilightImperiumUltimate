using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.DataAccess;

public static class ServiceCollectionExtensions
{
    private const string DbConnectionStringName = "TwilightImperium";

    public static IServiceCollection RegisterDataAccessLayer(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDbContext(configuration);
        services.RegisterRepositories();

        return services;
    }

    private static IServiceCollection RegisterDbContext(
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

        return services;
    }

    private static IServiceCollection RegisterRepositories(
        this IServiceCollection services)
    {
        services.AddScoped<ICardRepository, CardRepository>();
        services.AddScoped<IGalaxyRepository, GalaxyRepository>();
        services.AddScoped<IFactionRepository, FactionRepository>();
        services.AddScoped<INewsArticleRepository, NewsArticleRepository>();
        services.AddScoped<ITechnologyRepository, TechnologyRepository>();
        services.AddScoped<IRuleRepository, RuleRepository>();
        services.AddScoped<IWebsiteRepository, WebsiteRepository>();
        services.AddScoped<IFaqRepository, FaqRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IMapArchiveRepository, MapArchiveRepository>();
        services.AddScoped<ISlicesArchiveRepository, SlicesArchiveRepository>();
        services.AddScoped<IGameStatisticsRepository, GameStatisticsRepository>();
        services.AddTransient<IAsyncStatsRepository, AsyncStatsRepository>();
        services.AddScoped<ITiglRepository, TiglRepository>();
        services.AddScoped<ITiglUserRepository, TiglUserRepository>();
        services.AddScoped<ISeasonRepository, SeasonRepository>();
        services.AddScoped<IRatingDecayRepository, RatingDecayRepository>();
        services.AddScoped<IRankRepository, RankRepository>();
        services.AddScoped<IAchievementRepository, AchievementRepository>();
        services.AddScoped<IRankingsRepository, RankingsRepository>();

        return services;
    }
}
