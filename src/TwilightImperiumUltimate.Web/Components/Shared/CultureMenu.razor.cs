using Microsoft.AspNetCore.Components;
using System.Globalization;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class CultureMenu
{
    private bool _isMenuVisible;

    [Parameter]
    public bool InlineMode { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private void ToggleMenu()
    {
        _isMenuVisible = !_isMenuVisible;
    }

    private async Task SetCulture(string culture)
    {
        await CultureProvider.SetCultureAsync(culture);
        _isMenuVisible = false;

        // Little hack to force reload of the page
        NavigationManager.NavigateTo(NavigationManager.BaseUri);
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }

    private string GetCultureIconPath(string culture)
    {
        return PathProvider.GetCultureIconPath(culture);
    }

    private string GetCultureFlagClass(string culture)
    {
        return string.Equals(CultureInfo.CurrentCulture.Name, culture, StringComparison.OrdinalIgnoreCase)
            ? "active-culture-flag"
            : "inactive-culture-flag";
    }
}