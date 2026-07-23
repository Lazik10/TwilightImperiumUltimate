namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact ResponsiveSearchBar for use inside a ResponsiveTable/ResponsiveGrid cell (e.g. a
/// per-column filter trigger). Same contract as
/// <see cref="Bars.ResponsiveSearchBar"/>, just hiding the label and defaulting to the smallest
/// FontSize token so it fits a narrow column.
/// </summary>
public partial class TableActionSearchBar
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> OnSearchChange { get; set; }

    [Parameter]
    public string Text { get; set; } = Strings.SearchForKeyword;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets a value indicating whether the label is hidden. Defaults to true (opposite of
    /// ResponsiveSearchBar) since a full-text label rarely fits a table/grid column.
    /// </summary>
    [Parameter]
    public bool HideText { get; set; } = true;

    [Parameter]
    public bool EnableEmptySearch { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl). Defaults to "xs" -- smaller
    /// than ResponsiveSearchBar's own "md" default -- since this variant only ever sits inside a
    /// narrow table/grid cell.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "xs";
}
