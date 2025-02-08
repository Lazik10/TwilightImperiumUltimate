namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncTurnsStatsDto
{
    public AsyncTurnsStatsDto()
    {
    }

    public AsyncTurnsStatsDto(
        IReadOnlyCollection<AsyncTurnsPlayerDto> asyncFastestPlayers,
        IReadOnlyCollection<AsyncTurnsPlayerDto> asyncMostTurnsPlayers)
    {
        AsyncFastestPlayers = asyncFastestPlayers;
        AsyncMostTurnsPlayers = asyncMostTurnsPlayers;
    }

    public IReadOnlyCollection<AsyncTurnsPlayerDto> AsyncFastestPlayers { get; init; } = new List<AsyncTurnsPlayerDto>();

    public IReadOnlyCollection<AsyncTurnsPlayerDto> AsyncMostTurnsPlayers { get; init; } = new List<AsyncTurnsPlayerDto>();
}
