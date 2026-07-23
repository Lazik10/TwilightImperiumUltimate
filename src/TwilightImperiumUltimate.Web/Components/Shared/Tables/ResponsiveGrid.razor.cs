namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// A <see cref="ResponsiveTable{TItem}"/> that additionally supports: clicking a
/// <see cref="ResponsiveTableColumn{TItem}.Sortable"/> column header to sort ascending/descending/none,
/// single-row selection, and per-column filters rendered inline inside the column's own header cell
/// (rather than <see cref="ResponsiveTable{TItem}"/>'s separate filter row).
/// </summary>
/// <typeparam name="TItem">The row model type.</typeparam>
[CascadingTypeParameter(nameof(TItem))]
public partial class ResponsiveGrid<TItem>
{
    private ResponsiveTableColumn<TItem>? _sortColumn;
    private ResponsiveSortDirection _sortDirection = ResponsiveSortDirection.None;

    /// <summary>
    /// Gets or sets the currently selected row, if any.
    /// </summary>
    [Parameter]
    public TItem? SelectedItem { get; set; }

    /// <summary>
    /// Gets or sets a callback raised when the selected row changes. Clicking the selected row
    /// again clears the selection (passes null/default).
    /// </summary>
    [Parameter]
    public EventCallback<TItem?> SelectedItemChanged { get; set; }

    /// <summary>
    /// Gets or sets a callback raised whenever the sort column/direction changes, so a caller doing
    /// server-side sorting can re-fetch. Client-side sorting always happens regardless of whether
    /// this is set.
    /// </summary>
    [Parameter]
    public EventCallback<(string ColumnName, ResponsiveSortDirection Direction)> OnSortChanged { get; set; }

    /// <summary>
    /// Returns <paramref name="source"/> ordered by the current sort column/direction, or
    /// unchanged if nothing is currently sorted.
    /// </summary>
    private IEnumerable<TItem> ApplySort(IEnumerable<TItem> source)
    {
        if (_sortColumn is null || _sortDirection == ResponsiveSortDirection.None)
            return source;

        var column = _sortColumn;
        var comparer = Comparer<object?>.Create(CompareValues);

        return _sortDirection == ResponsiveSortDirection.Ascending
            ? source.OrderBy(item => column.Accessor(item), comparer)
            : source.OrderByDescending(item => column.Accessor(item), comparer);
    }

    private async Task ToggleSortAsync(ResponsiveTableColumn<TItem> column)
    {
        if (!column.Sortable)
            return;

        if (_sortColumn == column)
        {
            _sortDirection = _sortDirection switch
            {
                ResponsiveSortDirection.None => ResponsiveSortDirection.Ascending,
                ResponsiveSortDirection.Ascending => ResponsiveSortDirection.Descending,
                _ => ResponsiveSortDirection.None,
            };
        }
        else
        {
            _sortColumn = column;
            _sortDirection = ResponsiveSortDirection.Ascending;
        }

        if (_sortDirection == ResponsiveSortDirection.None)
            _sortColumn = null;

        if (OnSortChanged.HasDelegate)
            await OnSortChanged.InvokeAsync((column.PropertyName, _sortDirection));
    }

    private ResponsiveSortDirection GetSortDirection(ResponsiveTableColumn<TItem> column) =>
        _sortColumn == column ? _sortDirection : ResponsiveSortDirection.None;

    private string GetSortIndicator(ResponsiveTableColumn<TItem> column) => GetSortDirection(column) switch
    {
        ResponsiveSortDirection.Ascending => " \u25B2",
        ResponsiveSortDirection.Descending => " \u25BC",
        _ => string.Empty,
    };

    private string? GetAriaSort(ResponsiveTableColumn<TItem> column)
    {
        if (!column.Sortable)
            return null;

        return GetSortDirection(column) switch
        {
            ResponsiveSortDirection.Ascending => "ascending",
            ResponsiveSortDirection.Descending => "descending",
            _ => "none",
        };
    }

    private bool IsSelected(TItem item) => EqualityComparer<TItem>.Default.Equals(SelectedItem, item);

    private async Task SelectRowAsync(TItem item)
    {
        SelectedItem = IsSelected(item) ? default : item;
        await SelectedItemChanged.InvokeAsync(SelectedItem);
    }
}
