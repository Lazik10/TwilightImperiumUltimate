using TwilightImperiumUltimate.Web.Models.GameTracker;

namespace TwilightImperiumUltimate.Web.Services.GameTracker;

public interface IGameTrackerSettingsService
{
    int NumberOfPlayers { get; }

    int NumberOfPoints { get; }

    bool EnablePlayerNames { get; set; }

    IReadOnlyCollection<GameTrackerPlayerModel> Players { get; }

    IReadOnlyCollection<GameVersion> GameVersions { get; }

    Task SetNumberOfPlayers(int numberOfPlayers);

    Task SetNumberOfPoints(int numberOfPoints);

    Task SetGameVersion(GameVersion gameVersion, bool isEnabled);
}
