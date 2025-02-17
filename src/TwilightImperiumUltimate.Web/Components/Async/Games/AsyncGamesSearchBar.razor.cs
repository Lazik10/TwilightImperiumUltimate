namespace TwilightImperiumUltimate.Web.Components.Async.Games;

public partial class AsyncGamesSearchBar
{
    private string _selectedValue = string.Empty;

    [Parameter]
    public EventCallback<string> OnGameSelect { get; set; }

    [Parameter]
    public IReadOnlyCollection<string> GameNames { get; set; } = new List<string>();

    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string SearhTitle { get; set; } = "Search";

    private void SelectGame(string gameName)
    {
        _selectedValue = gameName;
        OnGameSelect.InvokeAsync(gameName);
    }

    private void ClearSelectedGame()
    {
        _selectedValue = string.Empty;
        OnGameSelect.InvokeAsync(string.Empty);
    }
}