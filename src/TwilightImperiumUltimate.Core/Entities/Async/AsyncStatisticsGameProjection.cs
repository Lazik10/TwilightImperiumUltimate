using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Async;

public sealed record AsyncStatisticsGameProjection(
    int GameStatsId,
    string AsyncGameId,
    string AsyncFunGameName,
    long Timestamp,
    long SetupTimestamp,
    long? EndedTimestamp,
    bool HasWinner,
    int NumberOfPlayers,
    int Scoreboard,
    bool IsTigl,
    bool AbsolMode,
    bool FrankenGame,
    long? DiscordUserId,
    AsyncFactionName? FactionName,
    int? Score,
    int? TotalNumberOfTurns,
    long? TotalTurnTime,
    float? ExpectedHits,
    float? ActualHits,
    bool? Eliminated,
    bool? Winner);
