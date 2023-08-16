namespace TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

public partial class TwilightImperiumDbContextInitializer
{
    private void InitializeEightTestPlayers()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var players = new List<Player>();

        for (int i = 1; i <= 8; i++)
        {
            players.Add(new Player()
            {
                Id = i,
                Name = $"TestPlayer{i}",
                Color = (PlayerColor)i,
            });
        }

        context.AddRange(players);
        context.SaveChanges();
    }
}