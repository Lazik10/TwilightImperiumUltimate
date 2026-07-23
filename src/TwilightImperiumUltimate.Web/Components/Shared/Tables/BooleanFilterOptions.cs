namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Holds the tri-state boolean filter options. Kept in a non-generic type so the array isn't
/// duplicated per closed generic instantiation of <see cref="ResponsiveColumnFilterControl{TItem}"/>.
/// </summary>
internal static class BooleanFilterOptions
{
    public static readonly bool?[] All = [null, true, false];
}
