namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class ScorePlayerIcon
{
    [Parameter]
    public FactionName FactionName { get; set; }

    /// <summary>
    /// Gets or sets the CSS filter value (e.g. "grayscale(100%)" or "") indicating whether this
    /// player has already scored, without the surrounding "filter:" / "-webkit-filter:" declarations.
    /// </summary>
    [Parameter]
    public string ScoreStatusFilter { get; set; } = string.Empty;

    [Parameter]
    public EventCallback OnClick { get; set; }

    private string GetFilterStyle() => $"filter: {ScoreStatusFilter}; -webkit-filter: {ScoreStatusFilter};";

    private async Task HandleClickAsync()
    {
        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync();
        }
    }
}
