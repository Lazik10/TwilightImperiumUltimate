namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class SystemTileRepository : ISystemTileRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context;

    private IReadOnlyList<SystemTile> _systemTiles = default!;

    public SystemTileRepository(IDbContextFactory<TwilightImperiumDbContext> context)
    {
        _context = context;
        InitializeSystemTiles().Wait();
    }

    public IReadOnlyList<SystemTile> GetAllSystemTiles() => _systemTiles;

    public IReadOnlyList<SystemTile> GetAnomalies() => _systemTiles.Where(x => x.HasAnomaly).ToList();

    public IReadOnlyList<SystemTile> GetBaseGameTiles() => _systemTiles.Where(x => x.GameVersion == GameVersion.BaseGame).ToList();

    public IReadOnlyList<SystemTile> GetHomeSystemTiles() => _systemTiles.Where(x => x.IsHomeSystem).ToList();

    public SystemTile GetMecatolRex() => _systemTiles.Single(x => x.SystemTileName == SystemTileName.Tile18);

    public SystemTile GetSystemTileByName(SystemTileName name) => _systemTiles.First(x => x.SystemTileName == name);

    public IReadOnlyList<SystemTile> GetWormholeTiles() => _systemTiles.Where(x => x.Wormholes.Count != 0).ToList();

    private async Task InitializeSystemTiles()
    {
        using var context = _context.CreateDbContext();

        _systemTiles = await context.SystemTiles.ToListAsync();
    }
}
