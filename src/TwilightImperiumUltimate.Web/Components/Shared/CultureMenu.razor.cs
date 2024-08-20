using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class CultureMenu
{
    private bool _isMenuVisible;

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
}