using Microsoft.JSInterop;
using Serilog;
using System.Globalization;

namespace TwilightImperiumUltimate.Web.Services.Language;

public class CultureProvider(IJSRuntime js) : ICultureProvider
{
    private readonly IJSRuntime _js = js;

    public async Task SetCultureAsync(string culture)
    {
        Log.Verbose("Setting application culture to: {Culture}", culture);
        await _js.InvokeVoidAsync("blazorCulture.set", culture);
        SetApplicationCulture(culture);
    }

    private static void SetApplicationCulture(string culture)
    {
        CultureInfo.CurrentCulture = new CultureInfo(culture);
        CultureInfo.CurrentUICulture = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
    }
}
