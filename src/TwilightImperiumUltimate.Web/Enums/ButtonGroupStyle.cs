namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Layout variants supported by <see cref="Components.Shared.Controls.ButtonGroup"/>. All
/// variants derive every button's width purely from CSS Grid track sizing (the widest button's
/// natural content width) -- no width is ever measured or hardcoded in C#.
/// </summary>
public enum ButtonGroupStyle
{
    /// <summary>
    /// Buttons sit in a single row, sized to their combined content (the group shrinks to fit,
    /// each button matching the width of the longest one).
    /// </summary>
    Row,

    /// <summary>
    /// Buttons sit in a single row that stretches to fill the available parent width, split
    /// evenly between all buttons.
    /// </summary>
    RowFill,

    /// <summary>
    /// Buttons stack in a single column, centered under one another, all matching the width of
    /// the widest button.
    /// </summary>
    Column,

    /// <summary>
    /// Buttons stack in a single column that stretches to fill the available parent width, each
    /// button spanning the full width of the group (not just the widest button's own content).
    /// </summary>
    ColumnFill,
}
