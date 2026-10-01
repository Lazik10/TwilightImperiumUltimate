using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.Business.Services.Async.Interfaces;
using TwilightImperiumUltimate.Contracts.DTOs.Async;

namespace TwilightImperiumUltimate.Business.Services.Async.Implementations;

public sealed class AsyncStatisticsSnapshotBuilder(
    IAsyncGeneralStatsFactory generalStatsFactory,
    IAsyncGamesStatsFactory gamesStatsFactory,
    IAsyncWinsStatsFactory winsStatsFactory,
    IAsyncVpStatsFactory vpStatsFactory,
    IAsyncEliminationsStatsFactory eliminationsStatsFactory,
    IAsyncTurnsStatsFactory turnsStatsFactory,
    IAsyncCombatStatsFactory combatStatsFactory,
    IAsyncDurationsStatsFactory durationsStatsFactory,
    IAsyncFactionStatsFactory factionStatsFactory,
    IAsyncOpponentsStatsFactory opponentsStatsFactory,
    IAsyncHistoryStatsFactory historyStatsFactory,
    ILogger<AsyncStatisticsSnapshotBuilder> logger)
    : IAsyncStatisticsSnapshotBuilder
{
    private const int CanonicalLimit = 200;
    private readonly ILogger<AsyncStatisticsSnapshotBuilder> _logger = logger;

    public async Task<AsyncStatisticsSnapshotDto> BuildAsync(CancellationToken cancellationToken)
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var generalTask = generalStatsFactory.CreateAsyncGeneralStatsSummary(cancellationToken);
        var gamesTask = gamesStatsFactory.CreateAsyncGamesStatsSummary(CanonicalLimit, cancellationToken);
        var winsTask = winsStatsFactory.CreateAsyncWinsStatsSummary(CanonicalLimit, cancellationToken);
        var vpTask = vpStatsFactory.CreateAsyncVpStatsSummary(CanonicalLimit, cancellationToken);
        var eliminationsTask = eliminationsStatsFactory.CreateAsyncEliminationsStatsSummary(CanonicalLimit, cancellationToken);
        var turnsTask = turnsStatsFactory.CreateAsyncTurnsStatsSummary(CanonicalLimit, cancellationToken);
        var combatTask = combatStatsFactory.CreateAsyncCombatStatsSummary(CanonicalLimit, cancellationToken);
        var durationsTask = durationsStatsFactory.CreateAsyncDurationsStatsSummary(CanonicalLimit, cancellationToken);
        var factionsTask = factionStatsFactory.CreateAsyncFactionStatsSummary(cancellationToken);
        var opponentsTask = opponentsStatsFactory.CreateAsyncOpponentsStatsSummary(CanonicalLimit, cancellationToken);
        var historyTask = historyStatsFactory.CreateAsyncHistoryStatsSummary(cancellationToken);

        await Task.WhenAll(
            generalTask,
            gamesTask,
            winsTask,
            vpTask,
            eliminationsTask,
            turnsTask,
            combatTask,
            durationsTask,
            factionsTask,
            opponentsTask,
            historyTask);

        var snapshot = new AsyncStatisticsSnapshotDto
        {
            General = await generalTask,
            Games = await gamesTask,
            Wins = await winsTask,
            VictoryPoints = await vpTask,
            Eliminations = await eliminationsTask,
            Turns = await turnsTask,
            Combat = await combatTask,
            Durations = await durationsTask,
            Factions = await factionsTask,
            Opponents = await opponentsTask,
            History = await historyTask,
        };

        AsyncStatisticsSnapshotValidator.Validate(snapshot);

        stopwatch.Stop();
        _logger.LogInformation("Async statistics snapshot built in {ElapsedMilliseconds} ms", stopwatch.ElapsedMilliseconds);

        return snapshot;
    }
}
