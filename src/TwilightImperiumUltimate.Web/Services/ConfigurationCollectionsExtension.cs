using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web.Settings.AppSettings;

namespace TwilightImperiumUltimate.Web.Services;

public static class ConfigurationCollectionsExtension
{
    public static WebAssemblyHostConfiguration RegisterConfigurations(
               this WebAssemblyHostConfiguration configuration)
    {
        configuration.Bind(nameof(TwilightImperiumApiSettings), new TwilightImperiumApiSettings());

        return configuration;
    }
}
