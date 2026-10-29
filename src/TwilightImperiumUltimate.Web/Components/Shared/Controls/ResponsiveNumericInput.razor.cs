namespace TwilightImperiumUltimate.Web.Components.Shared.Controls;

/// <summary>
/// Wraps a Radzen numeric input with the application's responsive input styling.
/// </summary>
public partial class ResponsiveNumericInput
{
    [Parameter]
    [EditorRequired]
    public string Id { get; set; } = string.Empty;

    [Parameter]
    public int Value { get; set; }

    [Parameter]
    public string? Name { get; set; }

    [Parameter]
    public string? AriaDescribedBy { get; set; }

    [Parameter]
    public string? AriaLabel { get; set; }

    [Parameter]
    public int Min { get; set; }

    [Parameter]
    public int? Max { get; set; }

    [Parameter]
    public int Step { get; set; } = 1;

    [Parameter]
    public bool Disabled { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public EventCallback<int> ValueChanged { get; set; }

    private string ComputedCssClass => $"responsive-numeric-input responsive-input-height handel {CssClass}".Trim();

    private string ComputedStyle => $"width: {Width}%; {Style}";

    private Dictionary<string, object> InputAttributes
    {
        get
        {
            var attributes = new Dictionary<string, object>();
            if (!string.IsNullOrWhiteSpace(AriaDescribedBy))
            {
                attributes["aria-describedby"] = AriaDescribedBy;
            }

            if (!string.IsNullOrWhiteSpace(AriaLabel))
            {
                // Radzen renders its own aria-label after InputAttributes; a label reference is preserved.
                attributes["aria-labelledby"] = $"{Id}-accessible-label";
            }

            return attributes;
        }
    }

    private async Task OnValueChanged(int value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(value);
    }
}
