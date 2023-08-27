using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web.Services.Language;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Services;

public static class ServiceCollectionsExtension
{
    public static IServiceCollection RegisterServices(
        this IServiceCollection services,
        WebAssemblyHostBuilder builder)
    {
        services.AddBlazoredLocalStorage();
        services.AddScoped<ICultureProvider, CultureProvider>();
        services.AddScoped<IPathProvider, PathProvider>();

        services.AddScoped(serviceProvider => new HttpClient
        {
            BaseAddress = new Uri(builder.HostEnvironment.BaseAddress),
        });

        return services;
    }
}
