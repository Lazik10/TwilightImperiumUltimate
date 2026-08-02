using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveEnumPicker<TEnum>
    where TEnum : Enum
{
    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public LabelPosition LabelPosition { get; set; } = LabelPosition.Row;

    [Parameter]
    public bool CenterLabel { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets an optional minimum width in pixels for the displayed value, so the arrows
    /// don't shift position as the value text changes length between enum members.
    /// </summary>
    [Parameter]
    public int? MinChoiceWidth { get; set; }

    [Parameter]
    public required TEnum CurrentValue { get; set; }

    [Parameter]
    public EventCallback<TEnum> CurrentValueChanged { get; set; }

    [Parameter]
    public List<TEnum> ExcludeValues { get; set; } = new List<TEnum>();

    /// <summary>
    /// Gets or sets the font size token (xs/sm/base/md/lg/xl/2xl/3xl, matching the site's
    /// --font-size-* scale) applied to both the label and the displayed value.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "md";

    [Parameter]
    public TextColor LabelColor { get; set; } = TextColor.White;

    [Parameter]
    public TextColor ValueColor { get; set; } = TextColor.Yellow;

    /// <summary>
    /// Gets or sets the label's width as a percentage, used only when <see cref="LabelPosition"/>
    /// is <see cref="Enums.LabelPosition.Row"/> (the remaining width goes to the +/- control).
    /// </summary>
    [Parameter]
    public int LabelWidth { get; set; } = 30;

    [Parameter]
    public int ButtonWidth { get; set; } = 10;

    private int PickerWidth => 100 - LabelWidth;

    private void NextValue()
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Except(ExcludeValues).ToArray();
        int currentIndex = Array.IndexOf(values, CurrentValue);
        int nextIndex = (currentIndex + 1) % values.Length;
        CurrentValue = values[nextIndex];
        CurrentValueChanged.InvokeAsync(CurrentValue);
    }

    private void PreviousValue()
    {
        var values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>().Except(ExcludeValues).ToArray();
        int currentIndex = Array.IndexOf(values, CurrentValue);
        int previousIndex = (currentIndex - 1 + values.Length) % values.Length;
        CurrentValue = values[previousIndex];
        CurrentValueChanged.InvokeAsync(CurrentValue);
    }
}
