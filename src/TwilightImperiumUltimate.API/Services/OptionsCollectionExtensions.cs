using TwilightImperiumUltimate.API.Options;
using TwilightImperiumUltimate.Contracts.Options;

namespace TwilightImperiumUltimate.API.Services;

public static class OptionsCollectionExtensions
{
    public static IServiceCollection RegisterOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.Configure<FrontendOptions>(configuration.GetSection("Frontend"));
        services.Configure<AsyncStatsOptions>(configuration.GetSection("AsyncStats"));
        services.Configure<StatsApiOptions>(configuration.GetSection("StatsApi"));
        services.Configure<DiscordOptions>(configuration.GetSection("Discord"));
        services.Configure<TiglOptions>(configuration.GetSection("Tigl"));

        return services;
    }
}
