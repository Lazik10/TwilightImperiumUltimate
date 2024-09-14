using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class BonusScoreGrid
{
    private ObjectiveCardName? _selectedObjectiveCard;
    private IReadOnlyCollection<KeyValuePair<ObjectiveCardName, string>> _objectives = default!;
    private IReadOnlyCollection<GameTrackerPlayerModel> _players = new List<GameTrackerPlayerModel>();

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        _players = GameTrackerService.Players;
        _objectives = ObjectiveCardTracker.Secrets
            .Where(x => x.ScoredFactions.Count > 0)
            .Select(x =>
                new KeyValuePair<ObjectiveCardName, string>(
                    x.ObjectiveCard.ObjectiveCardName, x.ObjectiveCard.ObjectiveCardName.GetCardDisplayName()))
            .ToList();
        _selectedObjectiveCard = ObjectiveCardTracker.Secrets
            .FirstOrDefault(x => x.IsLeakedSecret)?.ObjectiveCard.ObjectiveCardName;
    }

    private void ScoreAgendaBonusObjective(AgendaCardName agendaCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.AgendaCards.Contains(agendaCardName))
        {
            var bonusObjectives = player.BonusObjectives.AgendaCards.ToList();
            bonusObjectives.Remove(agendaCardName);
            player.BonusObjectives.AgendaCards = bonusObjectives;

            if (agendaCardName == AgendaCardName.Mutiny)
            {
                if (GameTrackerService.Mutiny)
                    player.Score--;
                else
                    player.Score++;
            }
            else
            {
                player.Score--;
            }
        }
        else
        {
            var bonusObjectives = player.BonusObjectives.AgendaCards.ToList();
            bonusObjectives.Add(agendaCardName);
            player.BonusObjectives.AgendaCards = bonusObjectives;

            if (agendaCardName == AgendaCardName.Mutiny)
            {
                if (GameTrackerService.Mutiny)
                    player.Score++;
                else
                    player.Score--;
            }
            else
            {
                player.Score++;
            }
        }

        StateHasChanged();
    }

    private void ScoreRelicBonusObjective(RelicCardName relicCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.Relics.Contains(relicCardName))
        {
            var bonusObjectives = player.BonusObjectives.Relics.ToList();
            bonusObjectives.Remove(relicCardName);
            player.BonusObjectives.Relics = bonusObjectives;
            player.Score--;
        }
        else
        {
            if (relicCardName == RelicCardName.ShardOfTheThrone || relicCardName == RelicCardName.TheCrownOfEmphidia)
            {
                foreach (var playerModel in _players.Where(player => player.BonusObjectives.Relics.Contains(relicCardName)))
                {
                    var playerModelBonusObjectives = playerModel.BonusObjectives.Relics.ToList();
                    playerModelBonusObjectives.Remove(relicCardName);
                    playerModel.BonusObjectives.Relics = playerModelBonusObjectives;
                    playerModel.Score--;
                }
            }

            var bonusObjectives = player.BonusObjectives.Relics.ToList();
            bonusObjectives.Add(relicCardName);
            player.BonusObjectives.Relics = bonusObjectives;
            player.Score++;
        }

        StateHasChanged();
    }

    private void ScoreActionBonusObjective(ActionCardName actionCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.ActionCards.Contains(actionCardName))
        {
            var bonusObjectives = player.BonusObjectives.ActionCards.ToList();
            bonusObjectives.Remove(actionCardName);
            player.BonusObjectives.ActionCards = bonusObjectives;
            player.Score--;
        }
        else
        {
            var bonusObjectives = player.BonusObjectives.ActionCards.ToList();
            bonusObjectives.Add(actionCardName);
            player.BonusObjectives.ActionCards = bonusObjectives;
            player.Score++;
        }

        StateHasChanged();
    }

    private void ScoreCustodians(GameTrackerPlayerModel player)
    {
        if (!player.BonusObjectives.CustodiansScored)
        {
            foreach (var otherPlayer in _players.Where(x => x.BonusObjectives.CustodiansScored))
            {
                otherPlayer.BonusObjectives.CustodiansScored = false;
                otherPlayer.Score--;
            }

            player.BonusObjectives.CustodiansScored = true;
            player.Score++;
        }
        else
        {
            player.BonusObjectives.CustodiansScored = false;
            player.Score--;
        }

        StateHasChanged();
    }

    private string GetCustodiansScoreStatus(GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.CustodiansScored)
        {
            return string.Empty;
        }
        else
        {
            return "grayscale(100%)";
        }
    }

    private string GetAgendaScoreStatus(AgendaCardName agendaCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.AgendaCards.Contains(agendaCardName))
        {
            return string.Empty;
        }
        else
        {
            return "grayscale(100%)";
        }
    }

    private string GetRelicScoreStatus(RelicCardName relicCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.Relics.Contains(relicCardName))
        {
            return string.Empty;
        }
        else
        {
            return "grayscale(100%)";
        }
    }

    private string GetActionScoreStatus(ActionCardName actionCardName, GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.ActionCards.Contains(actionCardName))
        {
            return string.Empty;
        }
        else
        {
            return "grayscale(100%)";
        }
    }

    private TextColor GetMutinyTextColor(bool forOrAgainst)
    {
        if (forOrAgainst && GameTrackerService.Mutiny)
        {
            return TextColor.Green;
        }

        if (!forOrAgainst && !GameTrackerService.Mutiny)
        {
            return TextColor.Red;
        }

        return TextColor.Grey;
    }

    private void UpdateMutiny(bool forOrAgainst)
    {
        if (forOrAgainst == GameTrackerService.Mutiny)
            return;

        GameTrackerService.Mutiny = forOrAgainst;

        // We need to not only remove the scored point or score penalty, but also add the opposite
        var scoreOffset = GameTrackerService.Mutiny ? 2 : -2;

        foreach (var player in _players.Where(player => player.BonusObjectives.AgendaCards.Contains(AgendaCardName.Mutiny)))
        {
            player.Score += scoreOffset;
        }

        StateHasChanged();
    }

    private async Task HandleClassifiedDocumentLeaks()
    {
        await ClearClassifiedDocumentLeaks(false);
        await ObjectiveCardTracker.UpdateClassifiedDocumentLeaks(_selectedObjectiveCard);
    }

    private async Task ClearClassifiedDocumentLeaks(bool clearSelectedObjectiveCard)
    {
        if (clearSelectedObjectiveCard)
            _selectedObjectiveCard = null;

        var secret = ObjectiveCardTracker.Secrets.FirstOrDefault(x => x.IsLeakedSecret);
        if (secret is not null)
        {
            secret.IsLeakedSecret = false;
            secret.Revealed = false;

            foreach (var player in _players
                .Where(player =>
                    player.ScoredObjectives
                        .Select(x =>
                            x.ObjectiveCardName)
                            .Contains(secret.ObjectiveCard.ObjectiveCardName)))
            {
                // Only clear players if they scored the leaked secret
                if (secret.ScoredFactions.Count > 0 && secret.ScoredFactions.ElementAt(0) != player.FactionName)
                {
                    var scoredObjectives = player.ScoredObjectives.ToList();
                    var updatedScoredObjectives = scoredObjectives.Where(x => x.ObjectiveCardName != secret.ObjectiveCard.ObjectiveCardName).ToList();
                    player.ScoredObjectives = updatedScoredObjectives;
                    player.Score--;
                    secret.RemoveScoredFaction(player.FactionName);
                }
            }
        }

        await ObjectiveCardTracker.ClearClassifiedDocumentLeaks();
        StateHasChanged();
    }

    private bool ShowClassifiedDocumnetLeaks()
    {
        return ObjectiveCardTracker.Secrets.Any(x => x.ScoredFactions.Count > 0);
    }

    private async Task HandleIncentiveProgram(ObjectiveCardType objectiveCardType)
    {
        if (ObjectiveCardTracker.IncentiveProgram == objectiveCardType)
        {
            await ResetPlayersIncentiveScore();
            await ObjectiveCardTracker.ResetIncentiveProgram(objectiveCardType);
            ObjectiveCardTracker.IncentiveProgram = null;
            return;
        }

        // When revealing a new incentive program, we need to reset the old one
        if (ObjectiveCardTracker.IncentiveProgram is not null)
        {
            await ResetPlayersIncentiveScore();
            await ObjectiveCardTracker.ResetIncentiveProgram((ObjectiveCardType)ObjectiveCardTracker.IncentiveProgram);
        }

        ObjectiveCardTracker.IncentiveProgram = objectiveCardType;
        await ObjectiveCardTracker.RevealIncentiveProgramCard(objectiveCardType);
    }

    private Task ResetPlayersIncentiveScore()
    {
        var lastCard = ObjectiveCardTracker.IncentiveProgram == ObjectiveCardType.StageOne
            ? ObjectiveCardTracker.DraftedStageOneObjectives.Last()
            : ObjectiveCardTracker.DraftedStageTwoObjectives.Last();

        var objectiveCardName = lastCard.ObjectiveCard.ObjectiveCardName;

        foreach (var player in _players.Where(player => player.ScoredObjectives.Select(x => x.ObjectiveCardName).Contains(objectiveCardName)))
        {
            var objectiveCard = player.ScoredObjectives.First(x => x.ObjectiveCardName == objectiveCardName);
            var updatedScoreObjectives = player.ScoredObjectives.Where(x => x.ObjectiveCardName != objectiveCardName).ToList();
            player.ScoredObjectives = updatedScoreObjectives;
            player.Score -= objectiveCard.ObjectiveCardType == ObjectiveCardType.StageOne ? 1 : 2;
        }

        return Task.CompletedTask;
    }

    private TextColor GetIncentiveProgramTextColor(ObjectiveCardType objectiveCardType)
    {
        if (objectiveCardType == ObjectiveCardTracker.IncentiveProgram)
        {
            return TextColor.Green;
        }

        return TextColor.Grey;
    }

    private void DecreasePlayerImperialScore(GameTrackerPlayerModel player)
    {
        if (player.BonusObjectives.ImperialScore > 0)
        {
            player.BonusObjectives.ImperialScore--;
            player.Score--;
        }
    }

    private void IncreasePlayerImperialScore(GameTrackerPlayerModel player)
    {
        player.BonusObjectives.ImperialScore++;
        player.Score++;
    }
}