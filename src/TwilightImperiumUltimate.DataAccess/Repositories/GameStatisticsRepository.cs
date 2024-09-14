using TwilightImperiumUltimate.Core.Entities.Statistics;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public class GameStatisticsRepository
    (IDbContextFactory<TwilightImperiumDbContext> context)
    : IGameStatisticsRepository
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _context = context;

    public async Task AddNewGameStatistics(Guid gameId, int points, int numberOfPlayers, int gameRound, string time, FactionName winner, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var gameStatistics = new GameStatistics
        {
            GameId = gameId,
            MaxPoints = points,
            NumberOfPlayers = numberOfPlayers,
            Round = gameRound,
            Time = time,
            Winner = winner,
        };

        dbContext.GameStatistics.Add(gameStatistics);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task AddNewRoundFactionStatistics(Guid gameId, int round, FactionName factionName, int score, StrategyCardName strategyCardName, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var factionRoundStatistics = new FactionRoundStatistics
        {
            GameId = gameId,
            Round = round,
            FactionName = factionName,
            Score = score,
            StrategyCardName = strategyCardName,
        };

        dbContext.FactionRoundStatistics.Add(factionRoundStatistics);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateWebsiteStatistics(StatisticsType type, CancellationToken cancellationToken)
    {
        await using var dbContext = await _context.CreateDbContextAsync(cancellationToken);

        var websiteStatistics = await dbContext.WebsiteStatistics
            .FirstOrDefaultAsync(cancellationToken);

        if (websiteStatistics is not null)
        {
            switch (type)
            {
                case StatisticsType.Visitors:
                    websiteStatistics.Visitors++;
                    break;
                case StatisticsType.MapsGenerated:
                    websiteStatistics.MapsGenerated++;
                    break;
                case StatisticsType.SlicesGenerated:
                    websiteStatistics.SlicesGenerated++;
                    break;
                case StatisticsType.MapsArchived:
                    websiteStatistics.MapsArchived++;
                    break;
                case StatisticsType.SlicesArchived:
                    websiteStatistics.SlicesArchived++;
                    break;
                case StatisticsType.GamesPlayed:
                    websiteStatistics.GamesPlayed++;
                    break;
                case StatisticsType.FactionsDrafted:
                    websiteStatistics.FactionsDrafted++;
                    break;
                case StatisticsType.ColorsDrafted:
                    websiteStatistics.ColorsDrafted++;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            dbContext.Update(websiteStatistics);
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
