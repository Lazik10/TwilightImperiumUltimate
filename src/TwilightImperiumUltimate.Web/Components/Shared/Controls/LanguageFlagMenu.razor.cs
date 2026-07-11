using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Reusable English/Czech language flag switcher used inside big-image overlays to switch which
/// localized image variant is shown. The parent owns the "current culture"/"current image" state
/// and reacts to <see cref="CultureSelected"/>.
/// </summary>
public partial class LanguageFlagMenu
{
    /// <summary>
    /// Gets or sets the culture currently shown, used to highlight the matching flag.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string CurrentCulture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional CSS classes applied to the wrapping element.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback invoked with the selected culture when a flag is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<string> CultureSelected { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetCultureIconPath(string culture) => PathProvider.GetCultureIconPath(culture);

    private string GetLanguageFlagClass(string culture) =>
        string.Equals(CurrentCulture, culture, StringComparison.OrdinalIgnoreCase)
            ? "language-flag-active"
            : "language-flag-inactive";

    private Task SelectCulture(string culture) => CultureSelected.InvokeAsync(culture);
}
