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

    private string GetTableClass() =>
        $"responsive-radzen-table handel white shadow {(ZebraStripes ? "responsive-radzen-table-zebra" : string.Empty)} {CssClass}".Trim();
}
