using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class SecretScoreGrid
{
    private IReadOnlyCollection<GameTrackerObjectiveCardModel> _secretObjectives = new List<GameTrackerObjectiveCardModel>();

    private IReadOnlyCollection<GameTrackerPlayerModel> _players = new List<GameTrackerPlayerModel>();

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    public Task Refresh()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        _secretObjectives = ObjectiveCardTracker.Secrets;
        _players = GameTrackerService.Players;
        StateHasChanged();
    }

    private async Task ScoreObjective(GameTrackerObjectiveCardModel objectiveCard, GameTrackerPlayerModel player)
    {
        await GameTrackerService.ScoreObjective(objectiveCard, player);
        StateHasChanged();
    }

    private string GetScoreStatus(GameTrackerObjectiveCardModel objectiveCard, GameTrackerPlayerModel player)
    {
        if (objectiveCard.ScoredFactions.Contains(player.FactionName))
        {
            return string.Empty;
        }
        else
        {
            return "grayscale(100%)";
        }
    }
}