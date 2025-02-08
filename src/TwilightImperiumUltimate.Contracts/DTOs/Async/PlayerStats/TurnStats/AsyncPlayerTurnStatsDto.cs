namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;

public record AsyncPlayerTurnStatsDto
{
    public AsyncPlayerTurnStatsDto()
    {
        TotalTurns = 0;
        TotalTurnTime = 0;
    }

    public AsyncPlayerTurnStatsDto(
        int totalTurns,
        long totalTurnTime)
    {
        TotalTurns = totalTurns;
        TotalTurnTime = totalTurnTime;
    }

    public int TotalTurns { get; init; }

    public long TotalTurnTime { get; init; }

    public float AverageTurnTime => TotalTurnTime == 0 ? 0 : (float)TotalTurnTime / TotalTurns;
}
