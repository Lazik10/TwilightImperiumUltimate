using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

/// <summary>
/// Responsive CSS Grid container. Supports a fixed Columns/TabletColumns/MobileColumns
/// breakpoint model (default, backward compatible), or an automatically reflowing
/// "auto-fit" layout when <see cref="MinItemWidth"/> is set. Per-cell and whole-grid
/// alignment use the same <see cref="Enums.AlignItems"/>/<see cref="Enums.JustifyContent"/>
/// enums as <see cref="ResponsiveContainer"/>, for consistency.
/// </summary>
public partial class ResponsiveGridContainer
{
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    [Parameter]
    public int Columns { get; set; } = 1;

    [Parameter]
    public int TabletColumns { get; set; }

    [Parameter]
    public int MobileColumns { get; set; }

    /// <summary>
    /// Gets or sets the minimum width of each column (for example "8rem"). When set, the grid
    /// switches to an `auto-fit` layout that automatically fits as many columns as the available
    /// width allows at any viewport size, instead of the fixed Columns/TabletColumns/MobileColumns
    /// breakpoints below.
    /// </summary>
    [Parameter]
    public string? MinItemWidth { get; set; }

    [Parameter]
    public string Gap { get; set; } = "var(--space-sm)";

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets the vertical alignment of content within each grid cell (CSS `align-items`).
    /// </summary>
    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.Center;

    /// <summary>
    /// Gets or sets the horizontal alignment of content within each grid cell (CSS `justify-items`).
    /// </summary>
    [Parameter]
    public AlignItems JustifyItems { get; set; } = AlignItems.Center;

    /// <summary>
    /// Gets or sets the horizontal alignment of the whole grid track area within the container,
    /// relevant when the grid's total column width is narrower than the container (CSS `justify-content`).
    /// </summary>
    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.Center;

    /// <summary>
    /// Gets or sets the vertical alignment of the whole grid track area within the container,
    /// relevant when the grid's total row height is shorter than the container (CSS `align-content`).
    /// </summary>
    [Parameter]
    public JustifyContent AlignContent { get; set; } = JustifyContent.Center;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private bool IsAutoFit => !string.IsNullOrWhiteSpace(MinItemWidth);

    private string GetContainerClass()
    {
        var classes = new List<string> { "responsive-grid-container" };

        if (IsAutoFit)
            classes.Add("responsive-grid-container--auto-fit");

        if (!string.IsNullOrWhiteSpace(CssClass))
            classes.Add(CssClass);

        return string.Join(" ", classes);
    }

    private string GetContainerStyle()
    {
        var tabletColumns = TabletColumns > 0 ? TabletColumns : Columns;
        var mobileColumns = MobileColumns > 0 ? MobileColumns : tabletColumns;

        return $"--grid-columns: {Columns}; --grid-columns-tablet: {tabletColumns}; --grid-columns-mobile: {mobileColumns}; " +
               $"--grid-min-item-width: {MinItemWidth}; --grid-gap: {Gap}; width: {Width}%; " +
               $"align-items: {AlignItems.GetAlignString()} justify-items: {JustifyItems.GetAlignString()} " +
               $"justify-content: {JustifyContent.GetJustifyString()} align-content: {AlignContent.GetJustifyString()} {Style}";
    }
}
