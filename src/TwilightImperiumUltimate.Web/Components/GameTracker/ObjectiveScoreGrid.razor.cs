using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class ObjectiveScoreGrid
{
    private IReadOnlyCollection<GameTrackerObjectiveCardModel> _stageOneObjectives = new List<GameTrackerObjectiveCardModel>();

    private IReadOnlyCollection<GameTrackerObjectiveCardModel> _stageTwoObjectives = new List<GameTrackerObjectiveCardModel>();

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

    protected override async Task OnInitializedAsync()
    {
        _stageOneObjectives = ObjectiveCardTracker.DraftedStageOneObjectives;
        _stageTwoObjectives = ObjectiveCardTracker.DraftedStageTwoObjectives;
        _players = GameTrackerService.Players;
        await ObjectiveCardTracker.RevealNextObjective(true);

        StateHasChanged();
    }

    private async Task ScoreObjective(GameTrackerObjectiveCardModel objectiveCard, GameTrackerPlayerModel player)
    {
        if (!objectiveCard.Revealed)
            return;

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

    private async Task RevealNextObjective()
    {
        await ObjectiveCardTracker.RevealNextObjective();
        StateHasChanged();
    }

    private void ManualPickChange(bool state)
    {
        ObjectiveCardTracker.ManualPickEnabled = state;
        StateHasChanged();
    }

    private bool ManualPickState()
    {
        return ObjectiveCardTracker.ManualPickEnabled;
    }

    private int GetColumnsCount() => Math.Max(ObjectiveCardTracker.DraftedStageOneObjectives.Count, ObjectiveCardTracker.DraftedStageTwoObjectives.Count);
}