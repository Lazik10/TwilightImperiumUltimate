namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Code-behind for <see cref="ResponsiveRow"/>.
/// </summary>
public partial class ResponsiveRow
{
    /// <summary>
    /// Gets or sets the row's cells (typically <see cref="ResponsiveColumn"/> instances).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets an optional color override for the row (e.g. to highlight a specific row).
    /// When not set, the row uses the table's default/zebra-striped background.
    /// </summary>
    [Parameter]
    public TextColor? Color { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this row is currently selected.
    /// </summary>
    [Parameter]
    public bool IsSelected { get; set; }

    /// <summary>
    /// Gets or sets the click callback, e.g. to select or navigate from this row.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

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

    private string ComputedCssClass =>
        $"responsive-row {(IsSelected ? "responsive-row-selected" : string.Empty)} {CssClass}".Trim();

    private string ComputedStyle
    {
        get
        {
            var cursorStyle = OnClick.HasDelegate ? "cursor: pointer;" : string.Empty;
            var colorStyle = Color is null ? string.Empty : $"background-color: {GetColorValue(Color.Value)}; --responsive-row-tint: {GetColorValue(Color.Value)};";
            return $"{cursorStyle} {colorStyle} {Style}";
        }
    }

    private static string GetColorValue(TextColor color) => color switch
    {
        TextColor.Red => "rgba(150, 30, 30, 0.35)",
        TextColor.Green => "rgba(30, 120, 30, 0.35)",
        TextColor.Yellow => "rgba(150, 140, 20, 0.35)",
        TextColor.Orange => "rgba(150, 90, 20, 0.35)",
        TextColor.Blue or TextColor.Deepskyblue or TextColor.LightBlue => "rgba(20, 80, 150, 0.35)",
        _ => "transparent",
    };
}
