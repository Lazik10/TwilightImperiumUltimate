namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncWinsStatsDto
{
    public AsyncWinsStatsDto()
    {
    }

    public AsyncWinsStatsDto(
        IReadOnlyCollection<AsyncWinsPlayerDto> asyncWinsPlayers,
        IReadOnlyCollection<AsyncWinsPlayerDto> asyncWinsPercentagePlayers,
        IReadOnlyCollection<AsyncWinsPlayerDto> asyncWinsDeviationPlayers)
    {
        AsyncWinsPlayers = asyncWinsPlayers;
        AsyncWinsPercentagePlayers = asyncWinsPercentagePlayers;
        AsyncWinsDeviationPlayers = asyncWinsDeviationPlayers;
    }

    public IReadOnlyCollection<AsyncWinsPlayerDto> AsyncWinsPlayers { get; init; } = new List<AsyncWinsPlayerDto>();

    public IReadOnlyCollection<AsyncWinsPlayerDto> AsyncWinsPercentagePlayers { get; init; } = new List<AsyncWinsPlayerDto>();

    public IReadOnlyCollection<AsyncWinsPlayerDto> AsyncWinsDeviationPlayers { get; init; } = new List<AsyncWinsPlayerDto>();
}

