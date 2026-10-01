namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

public partial class ResponsiveTextArea
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    [EditorRequired]
    public string Value { get; set; } = string.Empty;

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? Placeholder { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public int Rows { get; set; } = 8;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<string> ValueChanged { get; set; }

    private string ComputedCssClass => $"responsive-textarea handel {CssClass}".Trim();

    private string ComputedStyle => $"width: {Width}%; {Style}";

    private async Task OnValueChanged(string value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
