using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Web.Services.Async;

public interface IAsyncStatsProvider
{
    Task<AsyncGeneralSummaryStatsDto> GetGeneralStatistics();

    Task<AsyncGamesSummaryStatsDto> GetGamesStatistics(int limit);

    Task<AsyncWinsSummaryStatsDto> GetWinsStatistics(int limit);

    Task<AsyncVpSummaryStatsDto> GetVpStatistics(int limit);

    Task<AsyncEliminationsSummaryStatsDto> GetEliminationsStatistics(int limit);

    Task<AsyncTurnsSummaryStatsDto> GetTurnsStatistics(int limit);

    Task<AsyncCombatSummaryStatsDto> GetCombatStatistics(int limit);

    Task<AsyncDurationsSummaryStatsDto> GetDurationsStatistics(int limit);

    Task<AsyncOpponentsSummaryStatsDto> GetOpponentsStatistics(int limit);

    Task<AsyncFactionsSummaryStatsDto> GetFactionsStatistics();

    Task<AsyncHistorySummaryStatsDto> GetHistoryStatistics();
}
