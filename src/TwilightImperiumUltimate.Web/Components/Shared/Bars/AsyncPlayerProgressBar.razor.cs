namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class AsyncPlayerProgressBar
{
    [Parameter]
    public float Value { get; set; } = 50;

    [Parameter]
    public float MaxValue { get; set; } = 100;

    [Parameter]
    public float MinValue { get; set; } = 0;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int FontSize { get; set; } = 18;

    [Parameter]
    public TextColor TextColor { get; set; }

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private static bool AreFloatsEqual(float a, float b, float tolerance = 0.0001f)
    {
        return Math.Abs(a - b) < tolerance;
    }

    private double GetComputedValue()
    {
        // Fill the bar if the value is greater than the max
        if (Value >= MaxValue)
        {
            return 100;
        }

        // Show atleast a little
        if (AreFloatsEqual(Value, MinValue) || Value < MinValue)
        {
            return 0.5;
        }

        var range = MaxValue - MinValue;

        double adjustedValue = Value - MinValue;
        double percentage = (adjustedValue / range) * 100.0;

        return MinValue < MaxValue ? percentage : 0;
    }

    private string GetConvertedTextColor()
    {
        return TextColor switch
        {
            TextColor.Red => "red",
            TextColor.Yellow => "yellow",
            TextColor.Orange => "orange",
            TextColor.Green => "lawngreen",
            _ => "white",
        };
    }
}