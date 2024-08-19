using System;

namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class ProgressBar
{
    [Parameter]
    public int Value { get; set; } = 50;

    [Parameter]
    public int MaxValue { get; set; } = 100;

    [Parameter]
    public int MinValue { get; set; } = 0;

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int FontSize { get; set; } = 18;

    [Parameter]
    public string Color { get; set; } = string.Empty;

    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    private double GetComputedValue()
    {
        // Fill the bar if the value is greater than the max
        if (Value >= MaxValue)
        {
            return 100;
        }

        // Show atleast a little
        if (Value == MinValue)
        {
            return 2;
        }

        var range = MaxValue - MinValue;

        double adjustedValue = Value - MinValue;
        double percentage = (adjustedValue / range) * 100.0;

        return MinValue < MaxValue ? percentage : 0;
    }
}