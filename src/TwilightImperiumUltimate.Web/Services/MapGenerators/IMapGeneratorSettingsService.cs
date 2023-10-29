using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorSettingsService
{
    public int NumberOfPlayers { get; set; }

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }

    public GameVersion GameVersion { get; set; }

    public void IncreasePlayerCount();

    public void DecreasePlayerCount();
}
