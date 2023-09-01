using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using System.Globalization;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Helpers.Culture;

public static class CultureSetup
{
    public static async Task SetCultureAsync(this WebAssemblyHost? host)
    {
        ArgumentNullException.ThrowIfNull(host);

        var js = host.Services.GetRequiredService<IJSRuntime>();
        var result = await js.InvokeAsync<string>("blazorCulture.get");

        if (result is not null)
        {
            CultureInfo.CurrentCulture = new CultureInfo(result);
            CultureInfo.CurrentUICulture = new CultureInfo(result);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(result);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(result);
        }
        else
        {
            CultureInfo.CurrentCulture = new CultureInfo(Strings.EnglishCulture);
            CultureInfo.CurrentUICulture = new CultureInfo(Strings.EnglishCulture);
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(Strings.EnglishCulture);
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(Strings.EnglishCulture);
            await js.InvokeVoidAsync("blazorCulture.set", Strings.EnglishCulture);
        }
    }
}
