namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// A single selectable option shown in an <see cref="Enums.ResponsiveFilterType.Enum"/> column
/// filter dropdown.
/// </summary>
/// <param name="Display">The text shown to the user (the enum member's own name).</param>
/// <param name="Value">The underlying enum value.</param>
internal sealed record ResponsiveEnumFilterOption(string Display, object Value);
