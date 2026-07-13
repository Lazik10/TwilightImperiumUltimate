using System.Collections;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveDropDown<TValue>
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public TValue Value { get; set; } = default!;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public IEnumerable? Data { get; set; }

    [Parameter]
    public string? TextProperty { get; set; }

    [Parameter]
    public string? ValueProperty { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public bool AllowClear { get; set; }

    [Parameter]
    public bool AllowFiltering { get; set; }

    [Parameter]
    public bool AllowVirtualization { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl, matching the site's
    /// --font-size-* scale) applied to the underlying Radzen dropdown via its own
    /// --rz-input-font-size custom property.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "md";

    [Parameter]
    public EventCallback<TValue> ValueChanged { get; set; }

    private string ComputedCssClass => $"responsive-dropdown responsive-input-height handel {CssClass}".Trim();

    private string ComputedStyle => $"width: {Width}%; --rz-input-font-size: {GetFontSizeStyle()}; {Style}";

    private async Task OnValueChanged(TValue value)
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
