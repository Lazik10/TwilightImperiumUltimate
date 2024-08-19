namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class YesNoPicker
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<bool> OnValueChange { get; set; }

    [Parameter]
    public MarkupString LeftArrowSymbol { get; set; } = (MarkupString)Strings.ButtonLeftArrow;

    [Parameter]
    public MarkupString RightArrowSymbol { get; set; } = (MarkupString)Strings.ButtonRightArrow;

    [Parameter]
    public bool DefaultValue { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int FontSize { get; set; } = 18;

    public bool Value { get; set; }

    protected override void OnInitialized()
    {
        Value = DefaultValue;
    }

    private void ToggleValue()
    {
        Value = !Value;
        OnValueChange.InvokeAsync(Value);
    }

    private string GetDisplayValue()
    {
        return Value ? "Yes" : "No";
    }
}