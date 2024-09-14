using TwilightImperiumUltimate.Web.Components.GameTracker;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.Cards;

public partial class ObjectiveCard
{
    private IReadOnlyCollection<KeyValuePair<ObjectiveCardName, string>> _objectives = default!;
    private ObjectiveCardName _selectedObjectiveCard = ObjectiveCardName.AMassWealth;
    private ObjectiveCardName _previousObjectiveCard = ObjectiveCardName.AMassWealth;

    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Parameter]
    public ObjectiveCardName ObjectiveCardName { get; set; }

    [Parameter]
    public bool Picked { get; set; }

    [Parameter]
    public ObjectiveCardType ObjectiveCardType { get; set; }

    [Parameter]
    public bool DropDownEnabled { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [CascadingParameter(Name = "ObjectiveScoreGrid")]
    public ObjectiveScoreGrid ObjectiveScoreGrid { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    protected override void OnInitialized()
    {
        var allowedObjectives = ObjectiveCardTracker.ObjectiveCards.Where(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType).ToList();
        var allreadyRevealed = GetDraftedObjectiveCards();
        _objectives = allowedObjectives
            .Where(x => !allreadyRevealed.Contains(x.ObjectiveCard.ObjectiveCardName))
            .Select(x =>
                new KeyValuePair<ObjectiveCardName, string>(
                    x.ObjectiveCard.ObjectiveCardName, x.ObjectiveCard.ObjectiveCardName.GetCardDisplayName()))
            .ToList();
    }

    protected override void OnParametersSet()
    {
        _selectedObjectiveCard = ObjectiveCardName;
        _previousObjectiveCard = ObjectiveCardName;

        var allowedObjectives = ObjectiveCardTracker.ObjectiveCards.Where(x => x.ObjectiveCard.ObjectiveCardType == ObjectiveCardType).ToList();
        var allreadyRevealed = GetDraftedObjectiveCards();
        _objectives = allowedObjectives
            .Where(x => !allreadyRevealed.Contains(x.ObjectiveCard.ObjectiveCardName))
            .Select(x =>
                new KeyValuePair<ObjectiveCardName, string>(
                    x.ObjectiveCard.ObjectiveCardName, x.ObjectiveCard.ObjectiveCardName.GetCardDisplayName()))
            .ToList();
    }

    private List<ObjectiveCardName> GetDraftedObjectiveCards()
    {
        return ObjectiveCardType switch
        {
            ObjectiveCardType.StageOne => ObjectiveCardTracker.DraftedStageOneObjectives
                .Where(x => x.Revealed)
                .Select(x => x.ObjectiveCard.ObjectiveCardName)
                .ToList(),
            ObjectiveCardType.StageTwo => ObjectiveCardTracker.DraftedStageTwoObjectives
                .Where(x => x.Revealed)
                .Select(x => x.ObjectiveCard.ObjectiveCardName)
                .ToList(),
            _ => new List<ObjectiveCardName>(),
        };
    }

    private string GetCardImagePath()
    {
        var path = ObjectiveCardType switch
        {
            ObjectiveCardType.StageOne => Paths.ResourcePath_ObjectiveStageOne,
            ObjectiveCardType.StageTwo => Paths.ResourcePath_ObjectiveStageTwo,
            ObjectiveCardType.Secret => Paths.ResourcePath_ObjectiveSecret,
            _ => string.Empty,
        };

        if (Picked)
            return PathProvider.GetCardImagePath(Name, path);
        else
            return PathProvider.GetObjectiveCardBackPath(ObjectiveCardType);
    }

    private void HandleClick()
    {
        StateHasChanged();
    }

    private async Task HandleObjectiveChange()
    {
        if (_selectedObjectiveCard == _previousObjectiveCard)
            return;

        var objectiveForReset = ObjectiveCardTracker.ObjectiveCards.First(x => x.ObjectiveCard.ObjectiveCardName == _previousObjectiveCard);
        await GameTrackerService.ResetPlayersScoreForSpecificObjective(objectiveForReset);
        await ObjectiveCardTracker.UpdateObjectiveCards(_selectedObjectiveCard, _previousObjectiveCard);
        await ObjectiveScoreGrid.Refresh();
    }
}