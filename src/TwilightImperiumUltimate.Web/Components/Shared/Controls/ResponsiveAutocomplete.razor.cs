namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Thin wrapper around RadzenAutoComplete, matching ResponsiveTextBox/ResponsiveDropDown's
/// pattern (responsive input sizing, fluid font-size token), instead of a hand-rolled combobox.
/// Replaces the old, inaccessible <c>Shared/Bars/AutoComplete</c> component.
/// </summary>
public partial class ResponsiveAutocomplete
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public IEnumerable<string>? Data { get; set; }

    [Parameter]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl, matching the site's
    /// --font-size-* scale) applied to the underlying Radzen input via its own
    /// --rz-input-font-size custom property.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "md";

    private string ComputedCssClass => $"responsive-autocomplete responsive-input-height handel {CssClass}".Trim();

    private string ComputedStyle => $"width: {Width}%; --rz-input-font-size: {GetFontSizeStyle()}; {Style}";

    private async Task OnValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }

    private string GetFontSizeStyle() => FontSize switch
    {
        "xs" => "var(--font-size-xs)",
        "sm" => "var(--font-size-sm)",
        "base" => "var(--font-size-base)",
        "md" => "var(--font-size-md)",
        "lg" => "var(--font-size-lg)",
        "xl" => "var(--font-size-xl)",
        "2xl" => "var(--font-size-2xl)",
        "3xl" => "var(--font-size-3xl)",
        _ => "var(--font-size-md)",
    };
}
