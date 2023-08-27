using Blazored.LocalStorage;
using System.Globalization;
using TwilightImperiumUltimate.Web.Resources;

namespace TwilightImperiumUltimate.Web.Services.Language;

public class CultureProvider : ICultureProvider
{
    private readonly ILocalStorageService _localStorage;

    public CultureProvider(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<string> GetUserStoredCultureStringAsync()
    {
        var culture = await _localStorage.GetItemAsync<string>(Strings.CultureKey);
        return string.IsNullOrEmpty(culture) ? Strings.EnglishCulture : culture;
    }

    public async Task SetCultureAsync(string culture)
    {
        await _localStorage.SetItemAsync(Strings.CultureKey, culture);
        SetApplicationCulture(culture);
    }

    public async Task SetUserStoredCultureOrDefaultAsync()
    {
        var culture = await GetUserStoredCultureStringAsync();
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
