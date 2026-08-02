using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Grid;

/// <summary>
/// Renders a Radzen table/grid cell's content as a single whole-cell link instead of a
/// button-in-cell, so the entire column space (not just the text) is clickable/hoverable. Intended
/// for use inside a RadzenTableCell/RadzenDataGridColumn Template rendered by
/// ResponsiveRadzenTable/ResponsiveRadzenDataGrid.
/// </summary>
public partial class GridButtonColumn
{
    /// <summary>
    /// Gets or sets the text to display in the cell.
    /// </summary>
    [Parameter]
    public required string Text { get; set; }

    /// <summary>
    /// Gets or sets the destination URL to navigate to when the cell is clicked. When null or
    /// empty, the cell renders as plain non-interactive text instead of a link.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the text color.
    /// </summary>
    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets additional CSS classes to apply.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private string ComputedStyle => $"color: {TextColor.ConvertToString()};";
}
