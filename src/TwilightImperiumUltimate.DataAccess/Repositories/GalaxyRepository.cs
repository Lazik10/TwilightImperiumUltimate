namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class GalaxyRepository(
    IDbContextFactory<TwilightImperiumDbContext> context)
    : IGalaxyRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;
    private readonly Random _random = new();

    public async Task<SystemTile> GetSystemTileByName(SystemTileName name, CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Single(x => x.SystemTileName == name);
    }

    public async Task<SystemTile> GetMecatolRex(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Single(x => x.SystemTileName == SystemTileName.Tile18);
    }

    public async Task<SystemTile> GetHomePlaceholderSystemTile(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Single(x => x.SystemTileName == SystemTileName.TileHome);
    }

    public async Task<SystemTile> GetEmptyPlaceholderSystemTile(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Single(x => x.SystemTileName == SystemTileName.TileEmpty);
    }

    public async Task<IReadOnlyList<SystemTile>> GetAllSystemTiles(CancellationToken cancellationToken)
    {
        return await GetSystemTiles(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemTile>> GetHomeSystemTiles(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);

        var homeSystemTiles = systemTiles
            .Where(x => x.IsHomeSystem
                && x.SystemTileName != SystemTileName.Tile51
                && x.SystemTileName != SystemTileName.Tile92
                && x.SystemTileName != SystemTileName.Tile93
                && x.SystemTileName != SystemTileName.Tile94
                && x.SystemTileName != SystemTileName.Tile1036
                && x.SystemTileName != SystemTileName.Tile1035A
                && x.SystemTileName != SystemTileName.Tile1035B
                && x.SystemTileName != SystemTileName.Tile81)
            .ToList();

        var keleresSystemTiles = systemTiles
            .Where(x => x.SystemTileName == SystemTileName.Tile92
                || x.SystemTileName == SystemTileName.Tile93
                || x.SystemTileName == SystemTileName.Tile94)
            .OrderBy(x => _random.Next())
            .First();

        homeSystemTiles.Add(keleresSystemTiles);

        return homeSystemTiles;
    }

    public async Task<IReadOnlyList<SystemTile>> GetBaseGameSystemTiles(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Where(x => x.GameVersion == GameVersion.BaseGame).ToList();
    }

    public async Task<IReadOnlyList<SystemTile>> GetSystemTilesWithAnomalies(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Where(x => x.HasAnomaly).ToList();
    }

    public async Task<IReadOnlyList<SystemTile>> GetSystemTilesWithWormholes(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.Where(x => x.Wormholes.Count != 0).ToList();
    }

    public async Task<IReadOnlyList<SystemTile>> GetBlueSystemTiles(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles
            .Where(x => !x.IsHomeSystem
                && x.TileCategory != SystemTileCategory.None // Dummy tiles
                && x.SystemTileName != SystemTileName.Tile18 // Mecatol rex
                && x.SystemTileName != SystemTileName.Tile81 // Muaat supernova
                && x.TileCategory != SystemTileCategory.ExternalMapTile
                && x.TileCategory != SystemTileCategory.Hyperlane
                && x.TileCategory != SystemTileCategory.Red)
            .ToList();
    }

    public async Task<IReadOnlyList<SystemTile>> GetRedSystemTiles(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles
            .Where(x => x.TileCategory == SystemTileCategory.Red
                && !x.IsHomeSystem
                && x.FactionName == FactionName.None)
            .ToList();
    }

    public async Task<IReadOnlyList<Planet>> GetAllPlanets(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return await dbContext.Planets.ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<SystemTile>> GetAllHyperlines(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles
            .Where(x => x.TileCategory == SystemTileCategory.Hyperlane)
            .ToList();
    }

    public async Task<SystemTile> GetFrameSystemTile(CancellationToken cancellationToken)
    {
        var systemTiles = await GetSystemTiles(cancellationToken);
        return systemTiles.First(x => x.SystemTileName == SystemTileName.TileBlackFrame);
    }

    private async Task<List<SystemTile>> GetSystemTiles(CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);
        return dbContext.SystemTiles
            .Include(x => x.Planets)
            .Include(x => x.Wormholes)
            .AsEnumerable()
            .OrderBy(x => x.SystemTileName)
            .ToList();
    }
}
