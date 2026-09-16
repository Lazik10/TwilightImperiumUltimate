using TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

namespace TwilightImperiumUltimate.Contracts.DTOs.Async;

public sealed class AsyncStatisticsSnapshotDto
{
    public AsyncGeneralSummaryStatsDto General { get; set; } = new();

    public AsyncGamesSummaryStatsDto Games { get; set; } = new();

    public AsyncWinsSummaryStatsDto Wins { get; set; } = new();

    public AsyncVpSummaryStatsDto VictoryPoints { get; set; } = new();

    public AsyncEliminationsSummaryStatsDto Eliminations { get; set; } = new();

    public AsyncTurnsSummaryStatsDto Turns { get; set; } = new();

    public AsyncCombatSummaryStatsDto Combat { get; set; } = new();

    public AsyncDurationsSummaryStatsDto Durations { get; set; } = new();

    public AsyncFactionsSummaryStatsDto Factions { get; set; } = new();

    public AsyncOpponentsSummaryStatsDto Opponents { get; set; } = new();

    public AsyncHistorySummaryStatsDto History { get; set; } = new();
}
