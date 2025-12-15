namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncCombatStatsDto
{
    public AsyncCombatStatsDto()
    {
    }

    public AsyncCombatStatsDto(
        IReadOnlyCollection<AsyncCombatPlayerDto> totalHitsPlayers,
        IReadOnlyCollection<AsyncCombatPlayerDto> maxHitsPerGamePlayers,
        IReadOnlyCollection<AsyncCombatPlayerDto> maxAverageHitsPerGamePlayers,
        IReadOnlyCollection<AsyncCombatPlayerDto> bestHitsDeviationPlayers,
        IReadOnlyCollection<AsyncCombatPlayerDto> worstHitsDeviationPlayers)
    {
        TotalHitsPlayers = totalHitsPlayers;
        MaxHitsPerGamePlayers = maxHitsPerGamePlayers;
        MaxAverageHitsPerGamePlayers = maxAverageHitsPerGamePlayers;
        BestHitsDeviationPlayers = bestHitsDeviationPlayers;
        WorstHitsDeviationPlayers = worstHitsDeviationPlayers;
    }

    public IReadOnlyCollection<AsyncCombatPlayerDto> TotalHitsPlayers { get; init; } = new List<AsyncCombatPlayerDto>();

    public IReadOnlyCollection<AsyncCombatPlayerDto> MaxHitsPerGamePlayers { get; init; } = new List<AsyncCombatPlayerDto>();

    public IReadOnlyCollection<AsyncCombatPlayerDto> MaxAverageHitsPerGamePlayers { get; init; } = new List<AsyncCombatPlayerDto>();

    public IReadOnlyCollection<AsyncCombatPlayerDto> BestHitsDeviationPlayers { get; init; } = new List<AsyncCombatPlayerDto>();

    public IReadOnlyCollection<AsyncCombatPlayerDto> WorstHitsDeviationPlayers { get; init; } = new List<AsyncCombatPlayerDto>();
}
