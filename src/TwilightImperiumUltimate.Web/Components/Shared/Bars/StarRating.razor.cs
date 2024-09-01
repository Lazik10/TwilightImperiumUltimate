namespace TwilightImperiumUltimate.Web.Components.Shared.Bars;

public partial class StarRating
{
    private bool _isInitialRatingSet;

    [Parameter]
    public EventCallback<float> OnRatingChange { get; set; }

    [Parameter]
    public bool IsReadOnly { get; set; } = false;

    [Parameter]
    public float Rating { get; set; }

    [Parameter]
    public string NumberFormat { get; set; } = "F2";

    private Guid Guid { get; set; } = Guid.NewGuid();

    private void HandleClick(float rating)
    {
        if (IsReadOnly)
            return;

        OnRatingChange.InvokeAsync(rating);
    }

    private bool IsChecked(float value)
    {
        if (!IsReadOnly && !_isInitialRatingSet && ((Rating >= value - 0.25f && Rating < value + 0.25f) || Rating > value))
        {
            _isInitialRatingSet = true;
            return true;
        }
        else if (!IsReadOnly)
        {
            return false;
        }

        return (Rating >= value - 0.25f && Rating < value + 0.25f) || Rating > value;
    }
}