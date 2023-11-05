namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class SystemTileRepository : ISystemTileRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context;
    private readonly Random _random = new();

    private IReadOnlyList<SystemTile> _systemTiles = default!;

    public SystemTileRepository(IDbContextFactory<TwilightImperiumDbContext> context)
    {
        _context = context;
        InitializeSystemTiles().Wait();
    }

    public IReadOnlyList<SystemTile> GetAllSystemTiles() => _systemTiles;

    public IReadOnlyList<SystemTile> GetAnomalies() => _systemTiles.Where(x => x.HasAnomaly).ToList();

    public IReadOnlyList<SystemTile> GetBaseGameTiles() => _systemTiles.Where(x => x.GameVersion == GameVersion.BaseGame).ToList();

    public IReadOnlyList<SystemTile> GetHomeSystemTiles()
    {
        var homeSystemTiles = _systemTiles
        .Where(x => x.IsHomeSystem
            && x.SystemTileName != SystemTileName.Tile51
            && x.SystemTileName != SystemTileName.Tile92
            && x.SystemTileName != SystemTileName.Tile93
            && x.SystemTileName != SystemTileName.Tile94)
        .ToList();

        var keleresSystemTiles = _systemTiles
            .Where(x => x.SystemTileName == SystemTileName.Tile92
                        || x.SystemTileName == SystemTileName.Tile93
                        || x.SystemTileName == SystemTileName.Tile94)
            .OrderBy(x => _random.Next())
            .First();

        homeSystemTiles.Add(keleresSystemTiles);

        return homeSystemTiles;
    }

    public SystemTile GetMecatolRex() => _systemTiles.Single(x => x.SystemTileName == SystemTileName.Tile18);

    public SystemTile GetSystemTileByName(SystemTileName name) => _systemTiles.First(x => x.SystemTileName == name);

    public IReadOnlyList<SystemTile> GetWormholeTiles() => _systemTiles.Where(x => x.Wormholes.Count != 0).ToList();

    public IReadOnlyList<SystemTile> GetSystemTilesForBuildingGalaxy() => _systemTiles
        .Where(x => !x.IsHomeSystem
            && x.TileCategory != SystemTileCategory.None // Dummy tiles
            && x.SystemTileName != SystemTileName.Tile81 // Muaat supernova
            && x.SystemTileName != SystemTileName.Tile18 // Mecatol rex
            && x.TileCategory != SystemTileCategory.ExternalMapTile
            && x.TileCategory != SystemTileCategory.Hyperlance)
        .ToList();

    public SystemTile GetHomeSystemTilePlaceholderTile() => _systemTiles.First(x => x.SystemTileName == SystemTileName.TileHome);

    public SystemTile GetEmptySystemTilePlaceholderTile() => _systemTiles.First(x => x.SystemTileName == SystemTileName.TileEmpty);

    private async Task InitializeSystemTiles()
    {
        using var context = _context.CreateDbContext();

        _systemTiles = await context.SystemTiles.Include(x => x.Planets).ToListAsync();
    }
}
