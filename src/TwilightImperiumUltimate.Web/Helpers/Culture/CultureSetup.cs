using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Globalization;
using TwilightImperiumUltimate.Web.Services.Language;

namespace TwilightImperiumUltimate.Web.Helpers.Culture;

public static class CultureSetup
{
    public static async Task SetApplicationCultureAsync(this WebAssemblyHost? host)
    {
        ArgumentNullException.ThrowIfNull(host);

        var cultureProvider = host.Services.GetRequiredService<ICultureProvider>();
        var language = await cultureProvider.GetUserStoredCultureStringAsync();

        CultureInfo.CurrentCulture = new CultureInfo(language);
        CultureInfo.CurrentUICulture = new CultureInfo(language);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(language);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(language);
    }
}
