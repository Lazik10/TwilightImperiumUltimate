namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IGameStatisticsRepository
{
    Task AddNewRoundFactionStatistics(Guid gameId, int round, FactionName factionName, int score, StrategyCardName strategyCardName, CancellationToken cancellationToken);

    Task AddNewGameStatistics(Guid gameId, int points, int numberOfPlayers, int gameRound, string time, FactionName winner, CancellationToken cancellationToken);

    Task UpdateWebsiteStatistics(StatisticsType type, CancellationToken cancellationToken);
}
