using System.Globalization;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveNumericPicker
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Row;

    /// <summary>
    /// Gets or sets a value indicating whether the label text is centered. Defaults to true,
    /// matching the legacy NumericPicker's default (most existing call sites relied on that
    /// default rather than setting it explicitly).
    /// </summary>
    [Parameter]
    public bool CenterLabel { get; set; } = true;

    [Parameter]
    public int Value { get; set; }

    [Parameter]
    public EventCallback OnDecrease { get; set; }

    [Parameter]
    public EventCallback OnIncrease { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl, matching the site's
    /// --font-size-* scale) applied to both the label and the displayed value.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "lg";

    [Parameter]
    public string? DisplayText { get; set; }

    [Parameter]
    public TextColor LabelColor { get; set; } = TextColor.White;

    [Parameter]
    public TextColor ValueColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets the label's width as a percentage, used only when <see cref="LabelPosition"/>
    /// is <see cref="Enums.LabelPosition.Row"/> (the remaining width goes to the +/- control).
    /// </summary>
    [Parameter]
    public int LabelWidth { get; set; } = 30;

    [Parameter]
    public int ButtonWidth { get; set; } = 10;

    private int PickerWidth => 100 - LabelWidth;

    private string GetDisplayText() => string.IsNullOrEmpty(DisplayText) ? Value.ToString(CultureInfo.InvariantCulture) : DisplayText!;
}
