using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class ObjectivesGrid
{
    private List<GameTrackerObjectiveCardModel> _stageOneObjectives = new List<GameTrackerObjectiveCardModel>();

    private List<GameTrackerObjectiveCardModel> _stageTwoObjectives = new List<GameTrackerObjectiveCardModel>();

    [Parameter]
    public string Style { get; set; } = string.Empty;

    [CascadingParameter(Name = "GameTrackerGrid")]
    public GameTrackerGrid GameTrackerGrid { get; set; } = default!;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IGameTrackerSettingsService GameTrackerSettingsService { get; set; } = default!;

    public async Task Refresh()
    {
        await ObjectiveCardTracker.RevealNextObjective(true);
        _stageOneObjectives = ObjectiveCardTracker.DraftedStageOneObjectives.ToList();
        _stageTwoObjectives = ObjectiveCardTracker.DraftedStageTwoObjectives.ToList();
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        _stageOneObjectives = ObjectiveCardTracker.DraftedStageOneObjectives.ToList();
        _stageTwoObjectives = ObjectiveCardTracker.DraftedStageTwoObjectives.ToList();

        await GameTrackerGrid.Refresh();
        StateHasChanged();
    }

    private int GetNumberOfPlayers() => GameTrackerService.Players.Count;

    private int GetColumnsCount() => Math.Max(ObjectiveCardTracker.DraftedStageOneObjectives.Count, ObjectiveCardTracker.DraftedStageTwoObjectives.Count);
}