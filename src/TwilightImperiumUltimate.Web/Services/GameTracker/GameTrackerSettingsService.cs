using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Options.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public class GameTrackerSettingsService : IGameTrackerSettingsService
{
    private readonly List<GameTrackerPlayerModel> _players = GameTrackerOptions.DefaultGameTrackerPlayerModels.ToList();

    private readonly List<GameVersion> _gameVersions = GameTrackerOptions.GameVersions.ToList();

    public int NumberOfPlayers { get; private set; } = GameTrackerOptions.NumberOfPlayers;

    public int NumberOfPoints { get; private set; } = GameTrackerOptions.NumberOfPoints;

    public IReadOnlyCollection<GameTrackerPlayerModel> Players => _players;

    public IReadOnlyCollection<GameVersion> GameVersions => _gameVersions;

    public bool EnablePlayerNames { get; set; } = GameTrackerOptions.EnablePlayerNames;

    public Task SetNumberOfPlayers(int numberOfPlayers)
    {
        if (numberOfPlayers < 3 || numberOfPlayers > 8 || NumberOfPlayers == numberOfPlayers)
            return Task.CompletedTask;

        while (_players.Count > numberOfPlayers)
            _players.RemoveAt(_players.Count - 1);

        while (_players.Count < numberOfPlayers)
        {
            var playerNumber = _players.Count + 1;

            _players.Add(new GameTrackerPlayerModel
            {
                Id = playerNumber - 1,
                DefaultName = $"Player {playerNumber}",
                FactionName = FactionName.None,
                Initiative = InitiativeOrder.First,
                Score = 0,
            });
        }

        NumberOfPlayers = numberOfPlayers;
        return Task.CompletedTask;
    }

    public Task SetNumberOfPoints(int numberOfPoints)
    {
        if (numberOfPoints >= GameTrackerOptions.MinimumNumberOfPoints && numberOfPoints <= GameTrackerOptions.MaximumNumberOfPoints)
            NumberOfPoints = numberOfPoints;

        return Task.CompletedTask;
    }

    public Task SetGameVersion(GameVersion gameVersion, bool isEnabled)
    {
        if (isEnabled && !_gameVersions.Contains(gameVersion))
            _gameVersions.Add(gameVersion);
        else if (!isEnabled)
            _gameVersions.Remove(gameVersion);

        return Task.CompletedTask;
    }
}
