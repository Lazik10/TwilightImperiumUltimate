namespace TwilightImperiumUltimate.Web.Enums;

/// <summary>
/// Controls where a control's label is rendered relative to the control itself, used by
/// <see cref="Components.Shared.Controls.ResponsiveNumericPicker"/> and
/// <see cref="Components.Shared.Controls.ResponsiveFactionPicker"/>.
/// </summary>
public enum LabelPosition
{
    /// <summary>The label is rendered to the left of the control, both in the same row.</summary>
    Row,

    /// <summary>The label is rendered above the control, stacked in a column.</summary>
    Column,
}
