using System.Collections;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveDropDown<TValue>
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the label rendered above/beside the dropdown (row layout on desktop, stacked
    /// above the dropdown on narrow viewports) and used as the dropdown's accessible name. When
    /// omitted, no label is rendered and the host keeps its default "display: contents" layout.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

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

    /// <summary>
    /// Gets or sets whether the dropdown renders at <see cref="Width"/>% of its container (the
    /// default). Set to <see langword="false"/> to omit the percentage width entirely and let
    /// the dropdown size itself naturally -- needed inside auto-layout table cells, where a
    /// percentage width creates a circular/starved intrinsic-size computation that shrinks the
    /// dropdown far below its natural size (confirmed live for table column filters).
    /// </summary>
    [Parameter]
    public bool FillWidth { get; set; } = true;

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

    /// <summary>
    /// Gets or sets Radzen-specific parameters (e.g. <c>FilterCaseSensitivity</c>) that this
    /// wrapper does not declare explicitly, passed straight through to the underlying
    /// <c>RadzenDropDown</c> via <c>@attributes</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public IDictionary<string, object>? AdditionalAttributes { get; set; }

    private bool HasLabel => !string.IsNullOrWhiteSpace(Label);

    private string ComputedCssClass => $"responsive-dropdown responsive-input-height handel {CssClass}".Trim();

    private string ComputedStyle => $"{(FillWidth ? $"width: {Width}%; " : string.Empty)}--rz-input-font-size: {GetFontSizeStyle()}; {Style}";

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
