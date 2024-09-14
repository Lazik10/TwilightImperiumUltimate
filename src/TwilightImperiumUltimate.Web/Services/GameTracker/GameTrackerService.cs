using TwilightImperiumUltimate.Contracts.DTOs.Draft;
using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public class GameTrackerService(
    IGameTrackerSettingsService gameTrackerSettingsService,
    ITwilightImperiumApiHttpClient httpClient)
    : IGameTrackerService
{
    private readonly IGameTrackerSettingsService _gameTrackerSettingsService = gameTrackerSettingsService;
    private readonly ITwilightImperiumApiHttpClient _httpClient = httpClient;

    public Guid GameId { get; set; } = Guid.NewGuid();

    public int CurrentRound { get; set; } = 1;

    public GameTrackerPhase CurrentPhase { get; private set; } = GameTrackerPhase.Setup;

    public bool Mutiny { get; set; } = true;

    public GameTrackerPlayerModel ActivePlayer { get; set; } = new GameTrackerPlayerModel();

    public IReadOnlyCollection<GameTrackerPlayerModel> Players { get; private set; } = new List<GameTrackerPlayerModel>();

    public async Task<IReadOnlyDictionary<FactionName, PlayerColor>> GetCorrectColors(IReadOnlyCollection<FactionName> factions)
    {
        var colors = Enum.GetValues<PlayerColor>().ToList();

        ColorDraftRequest request = new() { Factions = factions, Colors = colors, };

        var (response, statusCode) = await _httpClient.PostAsync<ColorDraftRequest, ApiResponse<FactionColorDraftResultDto>>(Paths.ApiPath_ColorDraft, request);

        if (statusCode == HttpStatusCode.OK)
        {
            return response!.Data!.FactionColorDraftResults;
        }

        return new Dictionary<FactionName, PlayerColor>();
    }

    public Task InitializePlayers()
    {
        Players = _gameTrackerSettingsService.Players.ToList();

        var naaluPlayer = Players.FirstOrDefault(x => x.FactionName == FactionName.TheNaaluCollective);
        if (naaluPlayer is not null)
        {
            naaluPlayer.Initiative = InitiativeOrder.Zero;
        }

        return Task.CompletedTask;
    }

    public Task ResetPlayersScoreForSpecificObjective(GameTrackerObjectiveCardModel objectiveCard)
    {
        foreach (var player in Players.Where(player => player.ScoredObjectives.Select(x => x.ObjectiveCardName).Contains(objectiveCard.ObjectiveCard.ObjectiveCardName)))
        {
            player.Score -= objectiveCard.ScorePoints;
            player.ScoredObjectives = player.ScoredObjectives.Where(x => x.ObjectiveCardName != objectiveCard.ObjectiveCard.ObjectiveCardName).ToList();
        }

        return Task.CompletedTask;
    }

    public Task ScoreObjective(GameTrackerObjectiveCardModel objectiveCard, GameTrackerPlayerModel player)
    {
        var playerModel = Players.FirstOrDefault(x => x.FactionName == player.FactionName);
        if (playerModel is not null)
        {
            if (player.ScoredObjectives.Any(x => x.ObjectiveCardName == objectiveCard.ObjectiveCard.ObjectiveCardName))
            {
                var objectives = playerModel.ScoredObjectives.ToList();
                var scoredObjectiveCard = objectives.First(x => x.ObjectiveCardName == objectiveCard.ObjectiveCard.ObjectiveCardName);
                objectives.Remove(scoredObjectiveCard);
                playerModel.ScoredObjectives = objectives;
                playerModel.Score -= objectiveCard.ScorePoints;
                objectiveCard.RemoveScoredFaction(player.FactionName);
            }
            else
            {
                if (!objectiveCard.IsLeakedSecret
                    && objectiveCard.ObjectiveCard.ObjectiveCardType == ObjectiveCardType.Secret
                    && objectiveCard.ScoredFactions.Count >= 1)
                {
                    return Task.CompletedTask;
                }

                var objectives = playerModel.ScoredObjectives.ToList();
                objectives.Add(objectiveCard.ObjectiveCard);
                playerModel.ScoredObjectives = objectives;
                playerModel.Score += objectiveCard.ScorePoints;
                objectiveCard.AddScoredFaction(player.FactionName);
            }
        }

        return Task.CompletedTask;
    }

    public Task SetActivePlayer(GameTrackerPlayerModel player)
    {
        ActivePlayer = player;
        return Task.CompletedTask;
    }

    public void SetActivePlayerForAgenda()
    {
        ActivePlayer = new GameTrackerPlayerModel();
    }

    public Task SetGamePhase(GameTrackerPhase phase)
    {
        CurrentPhase = phase;
        return Task.CompletedTask;
    }
}
