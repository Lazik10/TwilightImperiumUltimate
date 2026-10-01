namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveDatePicker
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public DateTime Value { get; set; }

    [Parameter]
    public EventCallback<DateTime> ValueChanged { get; set; }

    [Parameter]
    public string DateFormat { get; set; } = "yyyy-MM-dd";

    [Parameter]
    public bool ShowTime { get; set; }

    [Parameter]
    public bool ShowSeconds { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string Style { get; set; } = string.Empty;

    private Task OnValueChanged(DateTime value)
    {
        Value = value;
        return ValueChanged.InvokeAsync(value);
    }
}
