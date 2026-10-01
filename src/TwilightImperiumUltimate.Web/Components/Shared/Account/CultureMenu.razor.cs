using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Web.Services.Language;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Account;

public partial class CultureMenu : IDisposable
{
    private bool _disposed;
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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            NavigationManager.LocationChanged -= OnLocationChanged;

        _disposed = true;
    }

    protected override void OnInitialized()
    {
        NavigationManager.LocationChanged += OnLocationChanged;
    }

    private void ToggleMenu()
    {
        _isMenuVisible = !_isMenuVisible;
    }

    private void CloseMenu()
    {
        _isMenuVisible = false;
    }

    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (_isMenuVisible && e.Key == "Escape")
        {
            _isMenuVisible = false;
        }
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

    private void OnLocationChanged(object? sender, LocationChangedEventArgs e)
    {
        // The non-inline flag dropdown is click-toggled (for keyboard accessibility) rather than
        // hover-driven, so it has no "click outside to close" behavior of its own. This component
        // lives outside the router-rendered content (in MainLayout), so it never gets torn
        // down/reset when the user navigates elsewhere (e.g. clicking InfoIcon) while it's open -
        // close it explicitly whenever the location changes.
        if (_isMenuVisible)
        {
            _isMenuVisible = false;
            InvokeAsync(StateHasChanged);
        }
    }
}
