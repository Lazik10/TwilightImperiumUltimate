using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace TwilightImperiumUltimate.Web.Services;

public static class ServiceCollectionsExtension
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        WebAssemblyHostBuilder builder)
    {
        services.AddScoped(serviceProvider => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        });

        return services;
    }
}
