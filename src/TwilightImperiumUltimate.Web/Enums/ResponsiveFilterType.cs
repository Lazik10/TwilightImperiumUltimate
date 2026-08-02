namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// The kind of filter control rendered for a <c>ResponsiveTableColumn</c> in
/// <c>ResponsiveTable</c>/<c>ResponsiveDataGrid</c>.
/// </summary>
public enum ResponsiveFilterType
{
    /// <summary>
    /// No filter control is rendered for the column.
    /// </summary>
    None,

    /// <summary>
    /// Case-insensitive "contains" text filter. Suitable for string columns.
    /// </summary>
    Text,

    /// <summary>
    /// Numeric filter with a comparison operator (equals, greater than, less than, etc.).
    /// Suitable for numeric columns.
    /// </summary>
    Value,

    /// <summary>
    /// Tri-state filter (all / true / false). Suitable for boolean columns.
    /// </summary>
    Boolean,

    /// <summary>
    /// Dropdown filter listing every value of the column's enum type. Suitable for enum columns.
    /// </summary>
    Enum,
}
