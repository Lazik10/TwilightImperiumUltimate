using TwilightImperiumUltimate.API.Options;

namespace TwilightImperiumUltimate.API.Services;

public static class OptionsCollectionExtensions
{
    public static IServiceCollection RegisterOptions(
               this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpOptions>(configuration.GetSection("Smtp"));
        services.Configure<FrontendOptions>(configuration.GetSection("Frontend"));

        return services;
    }
}
