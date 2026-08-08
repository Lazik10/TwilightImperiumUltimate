namespace TwilightImperiumUltimate.Web.Components.Factions.Menu;

/// <summary>
/// Generic, reusable slot grid: renders one row per entry in a breakpoint's column list (via
/// <see cref="ResponsiveGridContainer"/> per row), where each entry is that row's column count --
/// so rows can have different widths, and the number of rows is simply that list's length.
/// <see cref="Layouts"/> lets desktop, tablet, and mobile each define their own row/column shape
/// AND their own placeholder positions -- unlike a plain column-count change, repositioning
/// placeholders is a structural change and can't be done with CSS alone, so all three breakpoint
/// variants are rendered and only the one matching the current viewport width is shown (CSS
/// breakpoints match <see cref="ResponsiveGridContainer"/>: tablet max-width 1024px, mobile
/// max-width 768px). Only <see cref="ResponsiveBreakpoint.Desktop"/> is required in
/// <see cref="Layouts"/> -- <see cref="ResponsiveBreakpoint.Tablet"/> falls back to desktop and
/// <see cref="ResponsiveBreakpoint.Mobile"/> falls back to tablet when not supplied.
/// Slots are filled, in row-major order, with <see cref="Items"/>. Any slot index listed in a
/// layout's placeholder positions is skipped and rendered as an empty placeholder instead of
/// consuming an item -- this is what replaces the old, hand-rolled per-row "null = placeholder"
/// list-building logic previously duplicated across the faction menu components.
/// </summary>
/// <typeparam name="TItem">The type of item rendered into each non-placeholder slot.</typeparam>
public partial class FactionMenuGrid<TItem>
{
    private IReadOnlyDictionary<ResponsiveBreakpoint, IReadOnlyList<int>> _effectiveColumns =
        new Dictionary<ResponsiveBreakpoint, IReadOnlyList<int>>();

    private IReadOnlyDictionary<ResponsiveBreakpoint, IReadOnlyList<IReadOnlyList<GridSlot>>> _rowsByBreakpoint =
        new Dictionary<ResponsiveBreakpoint, IReadOnlyList<IReadOnlyList<GridSlot>>>();

    /// <summary>
    /// Gets or sets the per-breakpoint grid layout. Each entry's <c>Columns</c> list defines the
    /// row/column shape (list length = row count, each element = that row's column count) and
    /// <c>PlaceholderPositions</c> lists the zero-based, row-major slot indices that render as an
    /// empty placeholder instead of consuming an item. Must contain a
    /// <see cref="ResponsiveBreakpoint.Desktop"/> entry; <see cref="ResponsiveBreakpoint.Tablet"/>
    /// and <see cref="ResponsiveBreakpoint.Mobile"/> fall back to the nearest larger breakpoint when
    /// not supplied.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyDictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)> Layouts { get; set; } =
        new Dictionary<ResponsiveBreakpoint, (IReadOnlyList<int> Columns, IReadOnlyList<int> PlaceholderPositions)>();

    /// <summary>
    /// Gets or sets the items filled into the grid's slots, in row-major order, skipping any
    /// slot index present in the active breakpoint's placeholder positions.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyCollection<TItem> Items { get; set; } = Array.Empty<TItem>();

    [Parameter]
    [EditorRequired]
    public RenderFragment<TItem> ItemTemplate { get; set; } = default!;

    /// <summary>
    /// Gets or sets an optional custom placeholder render fragment. Falls back to a plain empty
    /// div (<c>faction-menu-grid-placeholder</c>) when not set.
    /// </summary>
    [Parameter]
    public RenderFragment? PlaceholderTemplate { get; set; }

    [Parameter]
    public string Gap { get; set; } = "var(--space-sm)";

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    protected override void OnParametersSet()
    {
        if (!Layouts.TryGetValue(ResponsiveBreakpoint.Desktop, out var desktopLayout))
        {
            throw new InvalidOperationException($"{nameof(Layouts)} must contain a {nameof(ResponsiveBreakpoint.Desktop)} entry.");
        }

        var tabletLayout = Layouts.TryGetValue(ResponsiveBreakpoint.Tablet, out var tablet) ? tablet : desktopLayout;
        var mobileLayout = Layouts.TryGetValue(ResponsiveBreakpoint.Mobile, out var mobile) ? mobile : tabletLayout;

        _effectiveColumns = new Dictionary<ResponsiveBreakpoint, IReadOnlyList<int>>
        {
            [ResponsiveBreakpoint.Desktop] = desktopLayout.Columns,
            [ResponsiveBreakpoint.Tablet] = tabletLayout.Columns,
            [ResponsiveBreakpoint.Mobile] = mobileLayout.Columns,
        };

        _rowsByBreakpoint = new Dictionary<ResponsiveBreakpoint, IReadOnlyList<IReadOnlyList<GridSlot>>>
        {
            [ResponsiveBreakpoint.Desktop] = BuildRows(desktopLayout.Columns, desktopLayout.PlaceholderPositions),
            [ResponsiveBreakpoint.Tablet] = BuildRows(tabletLayout.Columns, tabletLayout.PlaceholderPositions),
            [ResponsiveBreakpoint.Mobile] = BuildRows(mobileLayout.Columns, mobileLayout.PlaceholderPositions),
        };
    }

    private static ResponsiveBreakpoint[] GetBreakpointRenderOrder() => Enum.GetValues<ResponsiveBreakpoint>();

    private static string GetBreakpointCssClass(ResponsiveBreakpoint breakpoint) => breakpoint switch
    {
        ResponsiveBreakpoint.Desktop => "faction-menu-grid-desktop",
        ResponsiveBreakpoint.Tablet => "faction-menu-grid-tablet",
        ResponsiveBreakpoint.Mobile => "faction-menu-grid-mobile",
        _ => throw new ArgumentOutOfRangeException(nameof(breakpoint), breakpoint, message: null),
    };

    private string GetContainerCssClass() =>
        string.IsNullOrWhiteSpace(CssClass) ? "faction-menu-grid" : $"faction-menu-grid {CssClass}";

    private List<IReadOnlyList<GridSlot>> BuildRows(IReadOnlyList<int> columns, IReadOnlyList<int> placeholderPositions)
    {
        var placeholders = placeholderPositions.ToHashSet();
        var rows = new List<IReadOnlyList<GridSlot>>(columns.Count);

        using var itemsEnumerator = Items.GetEnumerator();

        var slotIndex = 0;

        foreach (var columnCount in columns)
        {
            var row = new List<GridSlot>(columnCount);

            for (var column = 0; column < columnCount; column++)
            {
                row.Add(placeholders.Contains(slotIndex) || !itemsEnumerator.MoveNext()
                    ? default
                    : GridSlot.WithItem(itemsEnumerator.Current));

                slotIndex++;
            }

            rows.Add(row);
        }

        return rows;
    }

    private readonly struct GridSlot
    {
        private GridSlot(TItem item, bool hasItem)
        {
            Item = item;
            HasItem = hasItem;
        }

        public TItem Item { get; }

        public bool HasItem { get; }

        public static GridSlot WithItem(TItem item) => new(item, true);
    }
}
