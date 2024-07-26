namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IGalaxyRepository
{
    Task<SystemTile> GetMecatolRex(CancellationToken cancellationToken);

    Task<SystemTile> GetHomePlaceholderSystemTile(CancellationToken cancellationToken);

    Task<SystemTile> GetEmptyPlaceholderSystemTile(CancellationToken cancellationToken);

    Task<SystemTile> GetSystemTileByName(SystemTileName name, CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetAllSystemTiles(CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetHomeSystemTiles(CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetBaseGameSystemTiles(CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetSystemTilesWithAnomalies(CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetSystemTilesWithWormholes(CancellationToken cancellationToken);

    Task<IReadOnlyList<SystemTile>> GetSystemTilesForBuildingGalaxy(CancellationToken cancellationToken);

    Task<IReadOnlyList<Planet>> GetAllPlanets(CancellationToken cancellationToken);
}
