namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Comparison operator used by a <see cref="ResponsiveFilterType.Value"/> column filter.
/// </summary>
public enum ResponsiveValueFilterOperator
{
    /// <summary>
    /// The column value equals the filter value.
    /// </summary>
    Equals,

    /// <summary>
    /// The column value is greater than the filter value.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// The column value is greater than or equal to the filter value.
    /// </summary>
    GreaterOrEqual,

    /// <summary>
    /// The column value is less than the filter value.
    /// </summary>
    LessThan,

    /// <summary>
    /// The column value is less than or equal to the filter value.
    /// </summary>
    LessOrEqual,
}
