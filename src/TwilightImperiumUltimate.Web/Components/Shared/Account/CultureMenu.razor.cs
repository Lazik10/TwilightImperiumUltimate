using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Services.Language;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Account;

public partial class CultureMenu
{
    private bool _isMenuVisible;

    [Parameter]
    public bool InlineMode { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private static IReadOnlyList<(string Code, string Label)> SupportedCultures =>
    [
        (Strings.EnglishCulture, "English"),
        (Strings.CzechCulture, "Czech"),
    ];

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private ICultureProvider CultureProvider { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

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
