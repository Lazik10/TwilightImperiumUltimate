using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Settings.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public class MapGeneratorSettingsService : IMapGeneratorSettingsService
{
    public int NumberOfPlayers { get; set; } = MapGeneratorSettings.NumberOfPlayers;

    public MapTemplate MapTemplate { get; set; } = MapGeneratorSettings.MapTemplate;

    public PlacementStyle PlacementStyle { get; set; } = MapGeneratorSettings.PlacementStyle;

    public SystemWeight SystemWeight { get; set; } = MapGeneratorSettings.SystemWeight;

    public GameVersion GameVersion { get; set; } = MapGeneratorSettings.GameVersion;

    public void IncreasePlayerCount()
    {
        if (NumberOfPlayers < MapGeneratorSettings.MaximumNumberOfPlayers)
        {
            NumberOfPlayers++;
        }
    }

    public void DecreasePlayerCount()
    {
        if (NumberOfPlayers > MapGeneratorSettings.MinimumNumberOfPlayers)
        {
            NumberOfPlayers--;
        }
    }
}
