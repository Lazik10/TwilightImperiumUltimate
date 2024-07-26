using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorSettingsService
{
    public int MapScale { get; set; }

    public MapTemplate MapTemplate { get; set; }

    public PlacementStyle PlacementStyle { get; set; }

    public SystemWeight SystemWeight { get; set; }

    public GameVersion GameVersion { get; set; }

    public SystemTileOverlay SystemTileOverlay { get; set; }

    public void IncreaseMapScale();

    public void DecreaseMapScale();
}
