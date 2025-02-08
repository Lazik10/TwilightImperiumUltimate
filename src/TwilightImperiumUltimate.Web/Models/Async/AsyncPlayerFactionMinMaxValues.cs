namespace TwilightImperiumUltimate.Web.Models.Async;

public class AsyncPlayerFactionMinMaxValues
{
    public AsyncPlayerFactionMinMaxValues()
    {
        Min = 0f;
        Value = 0f;
        Max = 0f;
    }

    public AsyncPlayerFactionMinMaxValues(float min, float value, float max)
    {
        Min = min;
        Value = value;
        Max = max;
    }

    public AsyncPlayerFactionMinMaxValues(float value, float max)
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
