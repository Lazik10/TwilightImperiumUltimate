namespace TwilightImperiumUltimate.Web.Models.Async;

public class AsyncFactionMinMaxValues
{
    public AsyncFactionMinMaxValues()
    {
        Min = 0f;
        Value = 0f;
        Max = 0f;
    }

    public AsyncFactionMinMaxValues(float min, float value, float max)
    {
        Min = min;
        Value = value;
        Max = max;
    }

    public AsyncFactionMinMaxValues(float value, float max)
    {
        Min = 0f;
        Value = value;
        Max = max;
    }

    public int Games { get; set; }

    public float Min { get; set; }

    public float Value { get; set; }

    public float Max { get; set; }
}
