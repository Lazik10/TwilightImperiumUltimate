namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveTextBox
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? AutoComplete { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets whether the input renders at <see cref="Width"/>% of its container (the
    /// default). Set to <see langword="false"/> to omit the percentage width entirely and let
    /// the input size itself naturally -- needed inside auto-layout table cells, where a
    /// percentage width creates a circular/starved intrinsic-size computation that shrinks the
    /// input far below its natural size (confirmed live for table column filters).
    /// </summary>
    [Parameter]
    public bool FillWidth { get; set; } = true;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl, matching the site's
    /// --font-size-* scale) applied to the underlying Radzen input via its own
    /// --rz-input-font-size custom property.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "md";

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    [Parameter]
    public EventCallback<string> InputChanged { get; set; }

    private string ComputedCssClass => $"responsive-textbox responsive-input-height handel {CssClass}".Trim();

    private string ComputedStyle => $"{(FillWidth ? $"width: {Width}%; " : string.Empty)}--rz-input-font-size: {GetFontSizeStyle()}; {Style}";

    private async Task OnValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }

    private async Task OnInput(ChangeEventArgs args)
    {
        var value = args.Value?.ToString() ?? string.Empty;
        Value = value;
        await InputChanged.InvokeAsync(value);
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
