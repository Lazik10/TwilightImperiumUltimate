using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class PlayerScoreCard
{
    [Parameter]
    public GameTrackerPlayerModel Player { get; set; } = new();

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    private string Border => $"border: solid 5px {Player.PlayerColor.GetUIColor()}";

    private string StrategyCardPath => PathProvider.GetCardImagePath(Player.StrategyCard.ToString(), Strings.StrategyCard);

    private string GetPassStatus() => Player.Passed ? "colorless" : string.Empty;

    private string GetStrategyCardStatus() => Player.StrategyCardUsed ? "colorless" : string.Empty;

    private void ActivatePlayer()
    {
        GameTrackerService.SetActivePlayer(Player);
        StateHasChanged();
    }

    private void StrategyCardClick()
    {
        if (Player.StrategyCardUsed && Player.Passed)
        {
            Player.StrategyCardUsed = false;
            Player.Passed = false;
        }
        else if (!Player.StrategyCardUsed)
        {
            Player.StrategyCardUsed = true;
        }
        else if (!Player.Passed)
        {
            Player.Passed = true;
        }

        StateHasChanged();
    }

    private string GetPlayerScoreBreakdown(ScoreType scoreType)
    {
        var stageOneCount = Player.ScoredObjectives.Count(x => x.ObjectiveCardType == ObjectiveCardType.StageOne);
        var stageTwoCount = Player.ScoredObjectives.Count(x => x.ObjectiveCardType == ObjectiveCardType.StageTwo);
        var secretsCount = Player.ScoredObjectives.Count(x => x.ObjectiveCardType == ObjectiveCardType.Secret);
        var bonusCount = Player.BonusObjectives.AgendaCards.Count + Player.BonusObjectives.Relics.Count + Player.BonusObjectives.ActionCards.Count;
        bonusCount += Player.BonusObjectives.CustodiansScored ? 1 : 0;
        bonusCount += Player.BonusObjectives.ImperialScore;

        return scoreType switch
        {
            ScoreType.StageOne => $"I: {stageOneCount}",
            ScoreType.StageTwo => $"II: {stageTwoCount}",
            ScoreType.Secret => $"S: {secretsCount}",
            ScoreType.Bonus => $"B: {bonusCount}",
            _ => string.Empty,
        };
    }
}