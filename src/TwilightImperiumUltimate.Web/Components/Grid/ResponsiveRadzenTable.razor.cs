namespace TwilightImperiumUltimate.Web.Components.Grid;

/// <summary>
/// Code-behind for <see cref="ResponsiveRadzenTable"/>.
/// </summary>
public partial class ResponsiveRadzenTable
{
    /// <summary>
    /// Gets or sets the table content, typically Radzen's own RadzenTableHeader/RadzenTableBody
    /// (each containing RadzenTableRow/RadzenTableCell) child components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public string TableTitle { get; set; } = string.Empty;

    [Parameter]
    public string TableDescription { get; set; } = string.Empty;

    [Parameter]
    public RenderFragment? TableTitleActions { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether odd/even rows are colored differently.
    /// </summary>
    [Parameter]
    public bool ZebraStripes { get; set; } = true;

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>Gets or sets the table width on desktop viewports.</summary>
    [Parameter]
    public string WidthDesktop { get; set; } = "100%";

    /// <summary>Gets or sets the table width on tablet viewports.</summary>
    [Parameter]
    public string WidthTablet { get; set; } = "100%";

    /// <summary>Gets or sets the table width on mobile viewports.</summary>
    [Parameter]
    public string WidthMobile { get; set; } = "100%";

    /// <summary>Gets or sets whether all columns use equal shares of the available table width.</summary>
    [Parameter]
    public bool EqualColumns { get; set; }

    private string GetTableClass() =>
        $"responsive-radzen-table handel white shadow {(ZebraStripes ? "responsive-radzen-table-zebra" : string.Empty)} {(EqualColumns ? "responsive-radzen-table-equal-columns" : string.Empty)} {CssClass}".Trim();

    private string GetHostStyle() =>
        $"--responsive-radzen-table-width-desktop: {WidthDesktop}; --responsive-radzen-table-width-tablet: {WidthTablet}; --responsive-radzen-table-width-mobile: {WidthMobile};";
}
