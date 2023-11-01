using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Models.Galaxy;
using TwilightImperiumUltimate.Web.Models.MapGenerators;

namespace TwilightImperiumUltimate.Web.Services.MapGenerators;

public interface IMapGeneratorService
{
    IReadOnlyCollection<SystemTile> SystemTiles { get; }

    Task<MapDraftResult> GenerateMapAsync();

    Task InitializeSystemTilesAsync();

    IReadOnlyCollection<SystemTile> GetSystemTilesToShow(SystemTileTypeFilter systemTileType);
}
