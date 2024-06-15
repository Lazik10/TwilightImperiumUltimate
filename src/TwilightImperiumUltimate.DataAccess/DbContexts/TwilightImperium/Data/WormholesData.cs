namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class WormholesData
{
    internal static List<Wormhole> Wormholes => GetWormholes();

    private static List<Wormhole> GetWormholes()
    {
        return
        [
            new() { Id = 1, SystemTileName = SystemTileName.Tile17, WormholeName = WormholeName.Delta, GameVersion = GameVersion.BaseGame },
            new() { Id = 2, SystemTileName = SystemTileName.Tile25, WormholeName = WormholeName.Beta, GameVersion = GameVersion.BaseGame },
            new() { Id = 3, SystemTileName = SystemTileName.Tile26, WormholeName = WormholeName.Alpha, GameVersion = GameVersion.BaseGame },
            new() { Id = 4, SystemTileName = SystemTileName.Tile39, WormholeName = WormholeName.Alpha, GameVersion = GameVersion.BaseGame },
            new() { Id = 5, SystemTileName = SystemTileName.Tile40, WormholeName = WormholeName.Beta, GameVersion = GameVersion.BaseGame },
            new() { Id = 6, SystemTileName = SystemTileName.Tile51, WormholeName = WormholeName.Delta, GameVersion = GameVersion.BaseGame },
            new() { Id = 7, SystemTileName = SystemTileName.Tile64, WormholeName = WormholeName.Beta, GameVersion = GameVersion.ProphecyOfKing },
            new() { Id = 8, SystemTileName = SystemTileName.Tile79, WormholeName = WormholeName.Alpha, GameVersion = GameVersion.ProphecyOfKing },
            new() { Id = 9, SystemTileName = SystemTileName.Tile82A, WormholeName = WormholeName.Gama, GameVersion = GameVersion.ProphecyOfKing },
            new() { Id = 10, SystemTileName = SystemTileName.Tile82B, WormholeName = WormholeName.Alpha, GameVersion = GameVersion.ProphecyOfKing },
            new() { Id = 11, SystemTileName = SystemTileName.Tile82B, WormholeName = WormholeName.Beta, GameVersion = GameVersion.ProphecyOfKing },
            new() { Id = 12, SystemTileName = SystemTileName.Tile82B, WormholeName = WormholeName.Gama, GameVersion = GameVersion.ProphecyOfKing },
        ];
    }
}
