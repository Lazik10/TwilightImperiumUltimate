namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface ISystemTileRepository
{
    SystemTile GetSystemTileByName(SystemTileName name);

    SystemTile GetMecatolRex();

    IReadOnlyList<SystemTile> GetAllSystemTiles();

    IReadOnlyList<SystemTile> GetHomeSystemTiles();

    IReadOnlyList<SystemTile> GetBaseGameTiles();

    IReadOnlyList<SystemTile> GetAnomalies();

    IReadOnlyList<SystemTile> GetWormholeTiles();

    IReadOnlyList<SystemTile> GetSystemTilesForBuildingGalaxy();

    SystemTile GetHomeSystemTilePlaceholderTile();

    SystemTile GetEmptySystemTilePlaceholderTile();
}
