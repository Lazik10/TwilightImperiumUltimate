using Radzen;
using Radzen.Blazor;

namespace TwilightImperiumUltimate.Web.Components.Grid;

/// <summary>
/// Thin wrapper around Radzen's <c>RadzenDataGrid</c> with this app's responsive styling
/// defaults. For a fully custom (non-Radzen) data grid with declarative
/// text/value/boolean/enum column filters and sticky columns, see
/// <see cref="TwilightImperiumUltimate.Web.Components.Shared.Tables.ResponsiveTable{TItem}"/> instead.
/// </summary>
/// <typeparam name="TItem">The row model displayed by the grid.</typeparam>
public partial class ResponsiveRadzenDataGrid<TItem>
{
    private RadzenDataGrid<TItem>? _grid;

    /// <summary>
    /// Gets or sets the data collection to display in the grid.
    /// </summary>
    [Parameter]
    public IEnumerable<TItem> Data { get; set; } = Enumerable.Empty<TItem>();

    /// <summary>
    /// Gets or sets a value indicating whether the grid displays its loading state while
    /// preserving the header and table frame.
    /// </summary>
    [Parameter]
    public bool IsLoading { get; set; }

    [Parameter]
    public string LoadingText { get; set; } = Strings.Loading;

    /// <summary>
    /// Gets or sets the column definitions for the grid.
    /// </summary>
    [Parameter]
    public RenderFragment? Columns { get; set; }

    [Parameter]
    public RenderFragment? HeaderTemplate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether filtering is enabled.
    /// </summary>
    [Parameter]
    public bool AllowFiltering { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether paging is enabled.
    /// </summary>
    [Parameter]
    public bool AllowPaging { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether sorting is enabled.
    /// </summary>
    [Parameter]
    public bool AllowSorting { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether odd/even rows are colored differently.
    /// </summary>
    [Parameter]
    public bool AllowAlternatingRows { get; set; }

    /// <summary>
    /// Gets or sets the number of items per page.
    /// </summary>
    [Parameter]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Gets or sets the horizontal alignment of the pager.
    /// </summary>
    [Parameter]
    public HorizontalAlign PagerHorizontalAlign { get; set; } = HorizontalAlign.Left;

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

    /// <summary>Gets or sets the grid width on desktop viewports.</summary>
    [Parameter]
    public string WidthDesktop { get; set; } = "100%";

    /// <summary>Gets or sets the grid width on tablet viewports.</summary>
    [Parameter]
    public string WidthTablet { get; set; } = "100%";

    /// <summary>Gets or sets the grid width on mobile viewports.</summary>
    [Parameter]
    public string WidthMobile { get; set; } = "100%";

    /// <summary>
    /// Gets or sets the default column width. Responsive by default.
    /// </summary>
    [Parameter]
    public string ColumnWidth { get; set; } = "auto";

    /// <summary>
    /// Gets or sets the callback invoked when a data row is clicked. When set, rows render with a
    /// pointer cursor and hover highlight to signal they are clickable.
    /// </summary>
    [Parameter]
    public EventCallback<TItem> RowClick { get; set; }

    /// <summary>
    /// Gets the combined CSS class string for the grid.
    /// </summary>
    /// <returns>The CSS class string.</returns>
    private string GetGridClass()
    {
        var clickableClass = RowClick.HasDelegate ? "responsive-radzen-grid-clickable-rows" : string.Empty;

        return $"responsive-radzen-grid {clickableClass} {CssClass}";
    }

    private Task OnRowClick(DataGridRowMouseEventArgs<TItem> args) => RowClick.InvokeAsync(args.Data);

    private string GetHostStyle() =>
        $"--responsive-radzen-grid-width-desktop: {WidthDesktop}; --responsive-radzen-grid-width-tablet: {WidthTablet}; --responsive-radzen-grid-width-mobile: {WidthMobile};";
}
