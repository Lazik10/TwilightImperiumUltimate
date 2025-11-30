using TwilightImperiumUltimate.Web.Options.Api;
using TwilightImperiumUltimate.Web.Options.Async;

namespace TwilightImperiumUltimate.Web.Services;

public static class OptionsCollectionsExtension
{
    public static IServiceCollection RegisterOptions(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TwilightImperiumApiOptions>(configuration.GetSection(nameof(TwilightImperiumApiOptions)));
        services.Configure<AsyncServerOptions>(configuration.GetSection(nameof(AsyncServerOptions)));
        return services;
    }
}
