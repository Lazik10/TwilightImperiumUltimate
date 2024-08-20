namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium.Data;

internal static class PlayersData
{
    internal static List<Player> Players => GetPlayers();

    private static List<Player> GetPlayers() => Enumerable.Range(1, 8)
        .Select(i => new Player() { Id = i, Name = $"TestPlayer{i}", Color = (PlayerColor)i, }).ToList();
}
