using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Mutable current filter value(s) for a single <c>ResponsiveTableColumn</c>. One instance is kept
/// per column by the owning <see cref="ResponsiveDataViewBase{TItem}"/>.
/// </summary>
public sealed class ResponsiveColumnFilterState
{
    /// <summary>
    /// Gets or sets the current text used by <see cref="ResponsiveFilterType.Text"/> and
    /// <see cref="ResponsiveFilterType.Value"/> filters.
    /// </summary>
    public string? TextValue { get; set; }

    /// <summary>
    /// Gets or sets the comparison operator used by a <see cref="ResponsiveFilterType.Value"/> filter.
    /// </summary>
    public ResponsiveValueFilterOperator ValueOperator { get; set; } = ResponsiveValueFilterOperator.Equals;

    /// <summary>
    /// Gets or sets the current tri-state value used by a <see cref="ResponsiveFilterType.Boolean"/> filter.
    /// Null means "all" (no filtering).
    /// </summary>
    public bool? BooleanValue { get; set; }

    /// <summary>
    /// Gets or sets the currently selected enum value used by a <see cref="ResponsiveFilterType.Enum"/> filter.
    /// Null means "all" (no filtering).
    /// </summary>
    public object? EnumValue { get; set; }
}
