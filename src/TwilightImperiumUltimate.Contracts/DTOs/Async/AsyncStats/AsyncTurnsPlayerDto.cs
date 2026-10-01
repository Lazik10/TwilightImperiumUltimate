namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncTurnsPlayerDto(
    int Id,
    string UserName,
    int Turns,
    long TotalTurnTime,
    int Games)
{
    public long AverageTurnTime => TotalTurnTime / Turns;
}
