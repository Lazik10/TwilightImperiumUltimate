using System.Globalization;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Shared engine for <c>ResponsiveTable{TItem}</c> and <c>ResponsiveGrid{TItem}</c>: column
/// registration, per-column filter state, and client-side filtering. Grid-only concerns (sorting,
/// selection) live in <c>ResponsiveGrid{TItem}</c> itself.
/// </summary>
/// <typeparam name="TItem">The row model type.</typeparam>
public abstract class ResponsiveDataViewBase<TItem> : ComponentBase, IResponsiveTableHost<TItem>
{
    /// <summary>
    /// The approximate pixel width assumed for a sticky column whose <see cref="ResponsiveTableColumn{TItem}.Width"/>
    /// is not a parseable pixel/rem value, used only to compute the left offset of subsequent sticky columns.
    /// </summary>
    private const int DefaultStickyColumnWidthPx = 120;

    private readonly List<ResponsiveTableColumn<TItem>> _columns = [];
    private readonly Dictionary<ResponsiveTableColumn<TItem>, ResponsiveColumnFilterState> _filterStates = [];
    private readonly string _instanceId = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets or sets the rows to display. In client-side mode this is the full/unfiltered list; in
    /// server-side mode (see <see cref="OnFilterChanged"/>) this is the already-filtered page
    /// supplied by the caller.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IEnumerable<TItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets the column declarations (<c>ResponsiveTableColumn</c> children).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether a loading state should be shown instead of the table.
    /// </summary>
    [Parameter]
    public bool IsLoading { get; set; }

    /// <summary>
    /// Gets or sets the loading message. Defaults to a plain "Loading..." string -- pass a
    /// localized <c>Strings.*</c> value for user-facing pages.
    /// </summary>
    [Parameter]
    public string LoadingText { get; set; } = "Loading...";

    /// <summary>
    /// Gets or sets the empty-state message shown when there are no rows to display. Defaults to a
    /// plain string -- pass a localized <c>Strings.*</c> value for user-facing pages.
    /// </summary>
    [Parameter]
    public string EmptyText { get; set; } = "No data found.";

    /// <summary>
    /// Gets or sets a value indicating whether odd/even rows are colored differently.
    /// </summary>
    [Parameter]
    public bool ZebraStripes { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the table scrolls horizontally instead of shrinking
    /// its columns when there isn't enough width. Required for <see cref="ResponsiveTableColumn{TItem}.Sticky"/>
    /// columns to have any effect.
    /// </summary>
    [Parameter]
    public bool EnableXScroll { get; set; } = true;

    /// <summary>
    /// Gets or sets a value indicating whether the header row stays visible while the table body
    /// scrolls vertically (requires a parent with a bounded height).
    /// </summary>
    [Parameter]
    public bool StickyHeader { get; set; } = true;

    /// <summary>
    /// Gets or sets an optional caption describing the table's contents, rendered above it for
    /// sighted and screen-reader users alike.
    /// </summary>
    [Parameter]
    public string? Caption { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes applied to the outer wrapper.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles applied to the outer wrapper.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the maximum width, in rem, the table is allowed to grow to. The table always
    /// stretches to 100% of its parent's width; when left null (the default) there is no cap.
    /// </summary>
    [Parameter]
    public double? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the visual gap between rows, e.g. "0.5rem" or "8px". When set, the table
    /// switches from collapsed to separated borders (<c>border-collapse: separate</c>) with that
    /// value as its vertical <c>border-spacing</c>, so each row reads as a distinct band with the
    /// wrapper's background showing through the gap. Left null (the default) for the normal
    /// flush/collapsed row layout.
    /// </summary>
    [Parameter]
    public string? RowGap { get; set; }

    /// <summary>
    /// Gets or sets an explicit row height, e.g. "3.5rem". Acts as a minimum -- a row still grows
    /// taller than this if its content needs more space. Left null (the default) to size rows
    /// purely from their content/padding.
    /// </summary>
    [Parameter]
    public string? RowHeight { get; set; }

    /// <summary>
    /// Gets or sets a callback raised whenever any column's filter value changes. Client-side
    /// filtering always happens regardless of whether this is set; attach a handler in addition to
    /// that when <see cref="Items"/> is a server-paged subset and a new page/filter needs to be
    /// fetched.
    /// </summary>
    [Parameter]
    public EventCallback OnFilterChanged { get; set; }

    /// <summary>
    /// Gets the currently registered columns, in declaration order.
    /// </summary>
    protected IReadOnlyList<ResponsiveTableColumn<TItem>> Columns => _columns;

    /// <summary>
    /// Gets the inline style applied to the outer wrapper: <see cref="Style"/> plus the
    /// <see cref="MaxWidth"/> cap (when set).
    /// </summary>
    protected string WrapperStyle => MaxWidth is > 0
        ? $"max-width: {MaxWidth.Value.ToString(CultureInfo.InvariantCulture)}rem; {Style}"
        : Style;

    /// <summary>
    /// Gets the inline style applied to the <c>&lt;table&gt;</c> element itself: <see cref="RowGap"/>
    /// (via <c>border-spacing</c>, inline so it reliably overrides the component's own non-important
    /// <c>border-collapse: collapse</c> base rule) and <see cref="RowHeight"/> (exposed as the
    /// <c>--responsive-row-height</c> custom property, read by each row's own CSS -- custom
    /// property values inherit through the DOM regardless of Blazor scope boundaries, so this
    /// reaches the child ResponsiveRow/ResponsiveGrid row components with no extra parameter needed).
    /// </summary>
    protected string TableStyle
    {
        get
        {
            var style = string.Empty;

            if (!string.IsNullOrWhiteSpace(RowGap))
                style += $"border-collapse: separate; border-spacing: 0 {RowGap}; ";

            if (!string.IsNullOrWhiteSpace(RowHeight))
                style += $"--responsive-row-height: {RowHeight};";

            return style;
        }
    }

    /// <summary>
    /// Gets a value indicating whether any column has a filter control.
    /// </summary>
    protected bool HasAnyFilter => _columns.Any(column => column.Filter != ResponsiveFilterType.None);

    void IResponsiveTableHost<TItem>.AddColumn(ResponsiveTableColumn<TItem> column)
    {
        if (_columns.Contains(column))
            return;

        _columns.Add(column);
        StateHasChanged();
    }

    void IResponsiveTableHost<TItem>.RemoveColumn(ResponsiveTableColumn<TItem> column)
    {
        _columns.Remove(column);
        _filterStates.Remove(column);
    }

    /// <summary>
    /// Compares two boxed column values for sorting, preferring <see cref="IComparable"/> and
    /// falling back to an ordinal string comparison.
    /// </summary>
    protected static int CompareValues(object? left, object? right)
    {
        if (left is null && right is null)
            return 0;

        if (left is null)
            return -1;

        if (right is null)
            return 1;

        if (left is IComparable comparable && left.GetType() == right.GetType())
            return comparable.CompareTo(right);

        return string.Compare(left.ToString(), right.ToString(), StringComparison.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Gets the mutable filter state for a column, creating it on first access.
    /// </summary>
    protected ResponsiveColumnFilterState GetFilterState(ResponsiveTableColumn<TItem> column)
    {
        if (!_filterStates.TryGetValue(column, out var state))
        {
            state = new ResponsiveColumnFilterState();
            _filterStates[column] = state;
        }

        return state;
    }

    /// <summary>
    /// Applies every column's current filter to <paramref name="source"/> and raises
    /// <see cref="OnFilterChanged"/>.
    /// </summary>
    protected async Task NotifyFilterChangedAsync()
    {
        if (OnFilterChanged.HasDelegate)
            await OnFilterChanged.InvokeAsync();
    }

    /// <summary>
    /// Returns <paramref name="source"/> narrowed down by every column's current filter value.
    /// </summary>
    protected IEnumerable<TItem> ApplyFilters(IEnumerable<TItem> source)
    {
        var result = source;

        foreach (var column in _columns)
        {
            if (column.Filter == ResponsiveFilterType.None || !_filterStates.TryGetValue(column, out var state))
                continue;

            result = column.Filter switch
            {
                ResponsiveFilterType.Text => FilterText(result, column, state),
                ResponsiveFilterType.Value => FilterValue(result, column, state),
                ResponsiveFilterType.Boolean => FilterBoolean(result, column, state),
                ResponsiveFilterType.Enum => FilterEnum(result, column, state),
                _ => result,
            };
        }

        return result;
    }

    /// <summary>
    /// Gets a stable id for a column, suitable for a filter input's "id" attribute.
    /// </summary>
    protected string GetColumnId(ResponsiveTableColumn<TItem> column) => $"{_instanceId}-{_columns.IndexOf(column)}";

    /// <summary>
    /// Gets the CSS class applied to a sticky column's cells.
    /// </summary>
    protected string GetStickyClass(ResponsiveTableColumn<TItem> column)
    {
        ArgumentNullException.ThrowIfNull(column);
        return column.Sticky && EnableXScroll ? "responsive-table-sticky-col" : string.Empty;
    }

    /// <summary>
    /// Gets the inline "position: sticky; left: …" style for a sticky column, offset by the width
    /// of every preceding sticky column.
    /// </summary>
    protected string GetStickyStyle(ResponsiveTableColumn<TItem> column)
    {
        ArgumentNullException.ThrowIfNull(column);

        if (!column.Sticky || !EnableXScroll)
            return string.Empty;

        var left = 0;
        foreach (var current in _columns)
        {
            if (current == column)
                break;

            if (current.Sticky)
                left += ParseWidthPixels(current.Width);
        }

        return string.Create(CultureInfo.InvariantCulture, $"position: sticky; left: {left}px; z-index: 2;");
    }

    /// <summary>
    /// Gets the full inline style for one of a column's cells: its <see cref="ResponsiveTableColumn{TItem}.Width"/>/
    /// <see cref="ResponsiveTableColumn{TItem}.MaxWidth"/> (applied to both header and body cells)
    /// plus the sticky-column positioning from <see cref="GetStickyStyle"/>.
    /// </summary>
    protected string GetColumnStyle(ResponsiveTableColumn<TItem> column)
    {
        ArgumentNullException.ThrowIfNull(column);

        var sizeStyle = string.Empty;

        if (!string.IsNullOrWhiteSpace(column.Width))
            sizeStyle += $"width: {column.Width}; ";

        if (!string.IsNullOrWhiteSpace(column.MaxWidth))
            sizeStyle += $"max-width: {column.MaxWidth}; overflow: hidden; text-overflow: ellipsis; ";

        return sizeStyle + GetStickyStyle(column);
    }

    private static int ParseWidthPixels(string? width)
    {
        if (string.IsNullOrWhiteSpace(width))
            return DefaultStickyColumnWidthPx;

        var digits = new string(width.Where(character => char.IsDigit(character) || character == '.').ToArray());
        return double.TryParse(digits, NumberStyles.Any, CultureInfo.InvariantCulture, out var value)
            ? (int)value
            : DefaultStickyColumnWidthPx;
    }

    private static IEnumerable<TItem> FilterText(IEnumerable<TItem> source, ResponsiveTableColumn<TItem> column, ResponsiveColumnFilterState state)
    {
        if (string.IsNullOrWhiteSpace(state.TextValue))
            return source;

        return source.Where(item => column.Accessor(item)?.ToString()?.Contains(state.TextValue, StringComparison.OrdinalIgnoreCase) == true);
    }

    private static IEnumerable<TItem> FilterValue(IEnumerable<TItem> source, ResponsiveTableColumn<TItem> column, ResponsiveColumnFilterState state)
    {
        if (string.IsNullOrWhiteSpace(state.TextValue) || !double.TryParse(state.TextValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var filterNumber))
            return source;

        return source.Where(item =>
        {
            var raw = column.Accessor(item);
            if (raw is null || !double.TryParse(Convert.ToString(raw, CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out var itemNumber))
                return false;

            return state.ValueOperator switch
            {
                ResponsiveValueFilterOperator.GreaterThan => itemNumber > filterNumber,
                ResponsiveValueFilterOperator.GreaterOrEqual => itemNumber >= filterNumber,
                ResponsiveValueFilterOperator.LessThan => itemNumber < filterNumber,
                ResponsiveValueFilterOperator.LessOrEqual => itemNumber <= filterNumber,
                _ => Math.Abs(itemNumber - filterNumber) < 0.0001,
            };
        });
    }

    private static IEnumerable<TItem> FilterBoolean(IEnumerable<TItem> source, ResponsiveTableColumn<TItem> column, ResponsiveColumnFilterState state)
    {
        if (state.BooleanValue is null)
            return source;

        return source.Where(item => column.Accessor(item) is bool value && value == state.BooleanValue);
    }

    private static IEnumerable<TItem> FilterEnum(IEnumerable<TItem> source, ResponsiveTableColumn<TItem> column, ResponsiveColumnFilterState state)
    {
        if (state.EnumValue is null)
            return source;

        return source.Where(item => Equals(column.Accessor(item), state.EnumValue));
    }
}
