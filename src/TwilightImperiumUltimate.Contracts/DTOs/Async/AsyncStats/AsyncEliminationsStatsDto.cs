namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncEliminationsStatsDto
{
    public AsyncEliminationsStatsDto()
    {
    }

    public AsyncEliminationsStatsDto(
        IReadOnlyCollection<AsyncEliminationsPlayerDto> mostEliminationsPlayers,
        IReadOnlyCollection<AsyncEliminationsPlayerDto> mostEliminationsPercentagePlayers)
    {
        MostEliminationsPercentagePlayers = mostEliminationsPercentagePlayers;
        MostEliminationsPlayers = mostEliminationsPlayers;
    }

    public IReadOnlyCollection<AsyncEliminationsPlayerDto> MostEliminationsPlayers { get; init; } = new List<AsyncEliminationsPlayerDto>();

    public IReadOnlyCollection<AsyncEliminationsPlayerDto> MostEliminationsPercentagePlayers { get; init; } = new List<AsyncEliminationsPlayerDto>();
}
