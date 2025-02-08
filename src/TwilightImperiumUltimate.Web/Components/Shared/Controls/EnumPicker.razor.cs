namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class EnumPicker<TEnum>
    where TEnum : Enum
{
    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int ButtonWidth { get; set; } = 100;

    [Parameter]
    public required TEnum CurrentValue { get; set; }

    [Parameter]
    public EventCallback<TEnum> CurrentValueChanged { get; set; }

    [Parameter]
    public List<TEnum> ExcludeValues { get; set; } = new List<TEnum>();

    [Parameter]
    public string Label { get; set; } = string.Empty;

    [Parameter]
    public bool CenterText { get; set; } = true;

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