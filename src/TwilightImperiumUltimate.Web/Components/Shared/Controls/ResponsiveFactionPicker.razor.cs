namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveFactionPicker
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Row;

    [Parameter]
    public bool CenterLabel { get; set; }

    [Parameter]
    public FactionName Value { get; set; }

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
    /// --font-size-* scale) applied to the label.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "lg";

    [Parameter]
    public TextColor LabelColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets the label's width as a percentage, used only when <see cref="LabelPosition"/>
    /// is <see cref="Enums.LabelPosition.Row"/> (the remaining width goes to the faction icon +
    /// arrow controls).
    /// </summary>
    [Parameter]
    public int LabelWidth { get; set; } = 30;

    private int PickerWidth => 100 - LabelWidth;
}
