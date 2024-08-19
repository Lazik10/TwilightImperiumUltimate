using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web.Options.Api;

namespace TwilightImperiumUltimate.Web.Services;

public static class OptionsCollectionsExtension
{
    public static WebAssemblyHostConfiguration RegisterOptions(
               this WebAssemblyHostConfiguration configuration)
    {
        configuration.Bind(nameof(TwilightImperiumApiOptions), new TwilightImperiumApiOptions());

        return configuration;
    }
}
