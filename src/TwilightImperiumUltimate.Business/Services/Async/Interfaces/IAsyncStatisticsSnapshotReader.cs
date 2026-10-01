using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Business.Services.Async.Interfaces;

public interface IAsyncStatisticsSnapshotReader
{
    void Invalidate();

    Task<AsyncGeneralSummaryStatsDto> GetGeneralAsync(CancellationToken cancellationToken);

    Task<AsyncGamesSummaryStatsDto> GetGamesAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncWinsSummaryStatsDto> GetWinsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncVpSummaryStatsDto> GetVictoryPointsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncEliminationsSummaryStatsDto> GetEliminationsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncTurnsSummaryStatsDto> GetTurnsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncCombatSummaryStatsDto> GetCombatAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncDurationsSummaryStatsDto> GetDurationsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncFactionsSummaryStatsDto> GetFactionsAsync(CancellationToken cancellationToken);

    Task<AsyncOpponentsSummaryStatsDto> GetOpponentsAsync(int limit, CancellationToken cancellationToken);

    Task<AsyncHistorySummaryStatsDto> GetHistoryAsync(CancellationToken cancellationToken);
}
