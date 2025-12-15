namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncDurationsStatsDto
{
    public AsyncDurationsStatsDto()
    {
    }

    public AsyncDurationsStatsDto(
        IReadOnlyCollection<AsyncDurationDto> longestGames,
        IReadOnlyCollection<AsyncDurationDto> fastestGames)
    {
        LongestGames = longestGames;
        FastestGames = fastestGames;
    }

    public IReadOnlyCollection<AsyncDurationDto> LongestGames { get; init; } = new List<AsyncDurationDto>();

    public IReadOnlyCollection<AsyncDurationDto> FastestGames { get; init; } = new List<AsyncDurationDto>();
}
