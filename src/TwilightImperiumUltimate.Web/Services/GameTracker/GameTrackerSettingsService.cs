using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Options.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public class GameTrackerSettingsService : IGameTrackerSettingsService
{
    private List<GameTrackerPlayerModel> _players = GameTrackerOptions.DefaultGameTrackerPlayerModels.ToList();

    private List<GameVersion> _gameVersions = GameTrackerOptions.GameVersions.ToList();

    public int NumberOfPlayers { get; private set; } = GameTrackerOptions.NumberOfPlayers;

    public int NumberOfPoints { get; private set; } = GameTrackerOptions.NumberOfPoints;

    public IReadOnlyCollection<GameTrackerPlayerModel> Players => _players;

    public IReadOnlyCollection<GameVersion> GameVersions => _gameVersions;

    public bool EnablePlayerNames { get; set; } = GameTrackerOptions.EnablePlayerNames;

    public Task DecreasePlayerCount()
    {
        if (NumberOfPlayers > 3)
        {
            NumberOfPlayers--;

            _players.RemoveAt(_players.Count - 1);
        }

        return Task.CompletedTask;
    }

    public Task DecreaseScorePoints()
    {
        if (NumberOfPoints > GameTrackerOptions.MinimumNumberOfPoints)
            NumberOfPoints--;

        return Task.CompletedTask;
    }

    public Task IncreasePlayerCount()
    {
        if (NumberOfPlayers < 8)
        {
            NumberOfPlayers++;

            _players.Add(new GameTrackerPlayerModel
            {
                Id = NumberOfPlayers - 1,
                DefaultName = $"Player {NumberOfPlayers}",
                FactionName = FactionName.None,
                Initiative = InitiativeOrder.First,
                Score = 0,
            });
        }

        return Task.CompletedTask;
    }

    public Task IncreaseScorePoints()
    {
        if (NumberOfPoints < GameTrackerOptions.MaximumNumberOfPoints)
            NumberOfPoints++;

        return Task.CompletedTask;
    }

    public Task UpdateGameVersion(GameVersion gameVersion)
    {
        if (!_gameVersions.Remove(gameVersion))
            _gameVersions.Add(gameVersion);

        return Task.CompletedTask;
    }
}
