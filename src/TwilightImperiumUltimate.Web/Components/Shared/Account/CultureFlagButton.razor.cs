using System.Globalization;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Account;

/// <summary>
/// A single selectable culture/language flag button used by <see cref="CultureMenu"/>.
/// </summary>
public partial class CultureFlagButton
{
    /// <summary>
    /// Gets or sets the culture code this button selects (for example "en-US").
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Culture { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the accessible name for this button (for example "English").
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the callback invoked with <see cref="Culture"/> when this button is activated.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback<string> OnCultureSelected { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetIconPath() => PathProvider.GetCultureIconPath(Culture);

    private string GetFlagClass() =>
        string.Equals(CultureInfo.CurrentCulture.Name, Culture, StringComparison.OrdinalIgnoreCase)
            ? "active-culture-flag"
            : "inactive-culture-flag";

    private Task HandleClick() => OnCultureSelected.InvokeAsync(Culture);
}
