using System.Globalization;

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

    /// <summary>
    /// Gets or sets the accessible name of the interactive star group (announced by screen readers
    /// before the individual star options). Ignored when <see cref="IsReadOnly"/> is set, because a
    /// read-only star row is decorative -- the numeric value next to it already conveys the rating.
    /// </summary>
    [Parameter]
    public string AriaLabel { get; set; } = string.Empty;

    private string GroupLabel => string.IsNullOrWhiteSpace(AriaLabel) ? Strings.StarRating_GroupLabel : AriaLabel;

    private Guid Guid { get; set; } = Guid.NewGuid();

    public void Refresh()
    {
        StateHasChanged();
    }

    private static string GetStarLabel(float value) =>
        string.Format(CultureInfo.CurrentCulture, Strings.StarRating_StarsLabel, value.ToString("0.#", CultureInfo.CurrentCulture));

    private void HandleClick(float rating)
    {
        if (IsReadOnly)
            return;

        Rating = rating;
        OnRatingChange.InvokeAsync(rating);
        _isInitialRatingSet = true;
        Refresh();
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
