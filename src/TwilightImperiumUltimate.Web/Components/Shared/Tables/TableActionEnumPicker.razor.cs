using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Tables;

/// <summary>
/// Compact ResponsiveEnumPicker for use inside a ResponsiveTable/ResponsiveGrid cell. Same
/// CurrentValue/ExcludeValues contract as
/// <see cref="Controls.ResponsiveEnumPicker{TEnum}"/>, just hiding the label and defaulting to
/// the smallest FontSize token so it fits a narrow column.
/// </summary>
/// <typeparam name="TEnum">The enum type being picked.</typeparam>
public partial class TableActionEnumPicker<TEnum>
    where TEnum : Enum
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Row;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public required TEnum CurrentValue { get; set; }

    [Parameter]
    public EventCallback<TEnum> CurrentValueChanged { get; set; }

    [Parameter]
    public List<TEnum> ExcludeValues { get; set; } = [];

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl). Defaults to "xs" -- smaller
    /// than ResponsiveEnumPicker's own "md" default -- since this variant only ever sits inside a
    /// narrow table/grid cell.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "xs";

    [Parameter]
    public TextColor LabelColor { get; set; } = TextColor.White;

    [Parameter]
    public TextColor ValueColor { get; set; } = TextColor.Yellow;

    [Parameter]
    public int LabelWidth { get; set; } = 30;

    [Parameter]
    public int ButtonWidth { get; set; } = 20;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public string Style { get; set; } = string.Empty;
}
