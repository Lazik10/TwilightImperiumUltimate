namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public interface IGameTrackerService
{
    GameTrackerPhase CurrentPhase { get; }

    Task<IReadOnlyDictionary<FactionName, PlayerColor>> GetCorrectColors(IReadOnlyCollection<FactionName> factions);

    Task StartGame();

    Task EndGame();

    Task SetGamePhase(GameTrackerPhase phase);

    Task PreviousPhase();

    Task UpdatePlayerScore(int playerIndex, int score);

    Task UpdatePlayerFaction(int playerIndex, FactionName factionName);

    Task UpdatePlayerInitiative(int playerIndex, InitiativeOrder initiative);

    Task UpdatePlayerName(int playerIndex, string name);
}
