using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class GameTrackerSettings
{
    private IReadOnlyCollection<GameTrackerPlayerModel> _players = new List<GameTrackerPlayerModel>();

    private IReadOnlyCollection<KeyValuePair<FactionName, string>> _factionNames = new Dictionary<FactionName, string>();

    [CascadingParameter(Name = "GameTrackerGrid")]
    public GameTrackerGrid GameTrackerGrid { get; set; } = default!;

    [Inject]
    private IGameTrackerSettingsService GameTrackerSettingsService { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    protected override void OnInitialized()
    {
        _factionNames = EnumExtensions.GetFactionValuesWithDisplayNames().OrderBy(x => x.Key.GetFactionUIText(FactionResourceType.Title)).ToDictionary();
        _players = GameTrackerSettingsService.Players;
    }

    private async Task IncreasePlayerCount()
    {
        await GameTrackerSettingsService.IncreasePlayerCount();
        _players = GameTrackerSettingsService.Players;
        StateHasChanged();
    }

    private async Task DecreasePlayerCount()
    {
        await GameTrackerSettingsService.DecreasePlayerCount();
        _players = GameTrackerSettingsService.Players;
        StateHasChanged();
    }

    private async Task IncreaseScorePoints()
    {
        await GameTrackerSettingsService.IncreaseScorePoints();
    }

    private async Task DecreaseScorePoints()
    {
        await GameTrackerSettingsService.DecreaseScorePoints();
    }

    private async Task UpdateGameVersion(GameVersion gameVersion)
    {
        await GameTrackerSettingsService.UpdateGameVersion(gameVersion);
    }

    private void TogglePlayerNames()
    {
        GameTrackerSettingsService.EnablePlayerNames = !GameTrackerSettingsService.EnablePlayerNames;
        StateHasChanged();
    }

    private async Task UpdatePlayerColors()
    {
        var colors = await GameTrackerService.GetCorrectColors(_players.Select(x => x.FactionName).ToList());

        foreach (var player in _players.Where(x => colors.Keys.Contains(x.FactionName)))
        {
            player.ColoTextName = colors[player.FactionName].GetUIColor();
            player.PlayerColor = colors[player.FactionName];
            player.IsColorPicked = true;
        }

        StateHasChanged();
    }

    private void HandleRectangleClick(GameTrackerPlayerModel player)
    {
        var currentColor = player.PlayerColor;
        var currentColorNumber = (int)currentColor;

        var totalNumberOfColors = Enum.GetValues<PlayerColor>().ToList().Count - 1;

        if (currentColorNumber < totalNumberOfColors)
        {
            currentColorNumber++;
            player.PlayerColor = (PlayerColor)currentColorNumber;
            player.ColoTextName = player.PlayerColor.GetUIColor();
        }
        else if (currentColorNumber == totalNumberOfColors)
        {
            currentColorNumber = 0;
            player.PlayerColor = (PlayerColor)currentColorNumber;
            player.ColoTextName = player.PlayerColor.GetUIColor();
        }

        player.IsColorPicked = true;

        StateHasChanged();
    }

    private bool IsNextPhasePossible()
    {
        return _players.All(x => x.FactionName != FactionName.None)
            && _players.Select(x => x.FactionName).Distinct().Count() == _players.Count
            && _players.All(x => x.IsColorPicked)
            && _players.Select(x => x.ColoTextName).Distinct().Count() == _players.Count;
    }

    private async Task ProceedToNextPhase()
    {
        await GameTrackerService.SetGamePhase(GameTrackerPhase.GameStarted);
        await ObjectiveCardTracker.InitializeRequiredCards();
        await ObjectiveCardTracker.DraftObjectiveCards();
        GameTrackerService.GameId = Guid.NewGuid();
        await GameTrackerGrid.Refresh();
    }
}