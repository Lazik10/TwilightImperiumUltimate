using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web.Options.Api;

namespace TwilightImperiumUltimate.Web.Services;

public static class ConfigurationCollectionsExtension
{
    public static WebAssemblyHostConfiguration RegisterConfigurations(
               this WebAssemblyHostConfiguration configuration)
    {
        configuration.Bind(nameof(TwilightImperiumApiOptions), new TwilightImperiumApiOptions());

        return configuration;
    }
}
