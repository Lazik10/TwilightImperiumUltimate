namespace TwilightImperiumUltimate.Core.Entities.Async;

public sealed record AsyncGeneralStatsGameProjection(
    int GameStatsId,
    bool IsTigl,
    long SetupTimestamp,
    long? EndedTimestamp,
    bool HasWinner,
    int NumberOfPlayers,
    int Round,
    int Scoreboard,
    long? DiscordUserId,
    bool? Eliminated,
    int? TotalNumberOfTurns,
    long? TotalTurnTime);
