namespace TwilightImperiumUltimate.Web.Components.Async;

public partial class AsyncMainGrid
{
    private AsyncMenuItem _selectedSegment = AsyncMenuItem.Statistics;

    [CascadingParameter(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Parameter]
    public AsyncMenuItem Segment { get; set; }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(Category))
        {
            if (Category == "statistics")
                _selectedSegment = AsyncMenuItem.Statistics;
            else if (Category == "players")
                _selectedSegment = AsyncMenuItem.Players;
            else if (Category == "games")
                _selectedSegment = AsyncMenuItem.Games;
        }

        StateHasChanged();
    }

    private void ChangeSegment(AsyncMenuItem segment)
    {
        _selectedSegment = segment;
        StateHasChanged();
    }
}