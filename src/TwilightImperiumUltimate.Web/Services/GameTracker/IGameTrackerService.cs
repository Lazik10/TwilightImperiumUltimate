using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public interface IGameTrackerService
{
    Guid GameId { get; set; }

    GameTrackerPhase CurrentPhase { get; }

    int CurrentRound { get; set; }

    bool Mutiny { get; set; }

    GameTrackerPlayerModel ActivePlayer { get; }

    IReadOnlyCollection<GameTrackerPlayerModel> Players { get; }

    Task<IReadOnlyDictionary<FactionName, PlayerColor>> GetCorrectColors(IReadOnlyCollection<FactionName> factions);

    Task SetGamePhase(GameTrackerPhase phase);

    Task InitializePlayers();

    Task SetActivePlayer(GameTrackerPlayerModel player);

    Task ScoreObjective(GameTrackerObjectiveCardModel objectiveCard, GameTrackerPlayerModel player);

    Task ResetPlayersScoreForSpecificObjective(GameTrackerObjectiveCardModel objectiveCard);

    void SetActivePlayerForAgenda();
}
