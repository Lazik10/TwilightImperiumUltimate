using System.Linq.Expressions;
using System.Reflection;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Declares a single column of a <c>ResponsiveTable{TItem}</c> or <c>ResponsiveDataGrid{TItem}</c>:
/// which property to display, its header text, its filter type, and whether it can be sorted or
/// is sticky when the table scrolls horizontally.
/// </summary>
/// <typeparam name="TItem">The row model type.</typeparam>
public partial class ResponsiveTableColumn<TItem> : IDisposable
{
    private Func<TItem, object?>? _compiledAccessor;
    private MemberInfo? _member;
    private bool _memberResolved;

    /// <summary>
    /// Gets or sets the property to display, e.g. <c>@(x => x.Name)</c>. Required.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public Expression<Func<TItem, object?>> Field { get; set; } = default!;

    /// <summary>
    /// Gets or sets the column header text. Defaults to the property name from <see cref="Field"/>.
    /// </summary>
    [Parameter]
    public string? Header { get; set; }

    /// <summary>
    /// Gets or sets the filter control rendered for this column. Defaults to no filter.
    /// </summary>
    [Parameter]
    public ResponsiveFilterType Filter { get; set; } = ResponsiveFilterType.None;

    /// <summary>
    /// Gets or sets a value indicating whether this column can be sorted. Only used by
    /// <c>ResponsiveDataGrid</c> -- ignored by <c>ResponsiveTable</c>, which never sorts.
    /// </summary>
    [Parameter]
    public bool Sortable { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the column header text is rendered vertically
    /// (rotated), useful for narrow columns with long headers.
    /// </summary>
    [Parameter]
    public bool Vertical { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this column stays pinned in place while the table
    /// scrolls horizontally. Only meaningful when the host's x-scroll is enabled.
    /// </summary>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Gets or sets the CSS width for the column, e.g. "10%" or "120px" -- applied as the actual
    /// rendered width of every cell in the column (both header and body), and (when
    /// <see cref="Sticky"/> is set) also used to compute how far subsequent sticky columns should
    /// be offset.
    /// </summary>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// Gets or sets the maximum CSS width for the column, e.g. "12rem" or "200px". Overflowing
    /// content is clipped with an ellipsis instead of stretching the column indefinitely.
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Gets or sets the text alignment for this column's body cells: "left", "center", or "right".
    /// Defaults to "left". Setting this to "center" also visually centers non-text cell content
    /// (e.g. a switch or button) since those render as inline-level elements that follow the
    /// cell's text-align.
    /// </summary>
    [Parameter]
    public string Align { get; set; } = "left";

    /// <summary>
    /// Gets or sets the text color used for this column's cells. Defaults to white.
    /// </summary>
    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets the text color used for this column's header cell. Defaults to the table's
    /// standard header accent color (<see cref="TextColor.LightBlue"/>) when not set, letting each
    /// column use a distinct header color when desired.
    /// </summary>
    [Parameter]
    public TextColor? HeaderTextColor { get; set; }

    /// <summary>
    /// Gets or sets the text alignment for this column's header cell: "left", "center", or
    /// "right". Defaults to "left".
    /// </summary>
    [Parameter]
    public string HeaderAlign { get; set; } = "left";

    /// <summary>
    /// Gets or sets additional CSS classes applied to every cell in this column.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets an optional custom cell template. When not provided, the raw property value
    /// (from <see cref="Field"/>) is rendered as text.
    /// </summary>
    [Parameter]
    public RenderFragment<TItem>? CellTemplate { get; set; }

    [CascadingParameter]
    internal IResponsiveTableHost<TItem>? Host { get; set; }

    /// <summary>
    /// Gets the resolved header text: <see cref="Header"/> if set, otherwise the property name.
    /// </summary>
    internal string ResolvedHeader => Header ?? PropertyName;

    /// <summary>
    /// Gets the property name extracted from <see cref="Field"/>, or an empty string if it could
    /// not be resolved (e.g. the expression is not a simple member access).
    /// </summary>
    internal string PropertyName => GetMember()?.Name ?? string.Empty;

    /// <summary>
    /// Gets the CLR type of the property extracted from <see cref="Field"/>. Used to enumerate the
    /// options for an <see cref="ResponsiveFilterType.Enum"/> filter.
    /// </summary>
    internal Type? PropertyType => GetMember() switch
    {
        PropertyInfo property => property.PropertyType,
        FieldInfo fieldInfo => fieldInfo.FieldType,
        _ => null,
    };

    /// <summary>
    /// Gets the compiled accessor used to read this column's value from a row.
    /// </summary>
    internal Func<TItem, object?> Accessor => _compiledAccessor ??= Field.Compile();

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    protected override void OnInitialized() => Host?.AddColumn(this);

    /// <summary>
    /// Unregisters this column from its host.
    /// </summary>
    /// <param name="disposing">Whether this is being called from <see cref="Dispose()"/>.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
            Host?.RemoveColumn(this);
    }

    private MemberInfo? GetMember()
    {
        if (_memberResolved)
            return _member;

        _memberResolved = true;

        var body = Field.Body;
        if (body is UnaryExpression unary)
            body = unary.Operand;

        _member = (body as MemberExpression)?.Member;
        return _member;
    }
}
