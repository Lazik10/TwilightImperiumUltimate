namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact ResponsiveAutocomplete for use inside a ResponsiveTable cell. Same
/// contract as <see cref="Controls.ResponsiveAutocomplete"/>, just defaulting to the smallest
/// FontSize token and a shorter input height so it fits a narrow column.
/// </summary>
public partial class TableActionAutocomplete
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
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl). Defaults to "xs" -- smaller
    /// than ResponsiveAutocomplete's own "md" default -- since this variant only ever sits inside
    /// a narrow table/grid cell.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "xs";

    private async Task OnValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
