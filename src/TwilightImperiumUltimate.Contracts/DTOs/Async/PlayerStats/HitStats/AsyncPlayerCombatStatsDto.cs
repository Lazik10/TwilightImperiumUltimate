namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;

public record AsyncPlayerCombatStatsDto
{
    public AsyncPlayerCombatStatsDto()
    {
    }

    public AsyncPlayerCombatStatsDto(
       float expectedHits,
       float actualHits,
       float minHitsPerGame,
       float maxHitsPerGame,
       float averageHitsPerGame)
    {
        ExpectedHits = expectedHits;
        ActualHits = actualHits;
        MinHitsPerGame = minHitsPerGame;
        MaxHitsPerGame = maxHitsPerGame;
        AverageHitsPerGame = averageHitsPerGame;
    }

    public float ExpectedHits { get; set; }

    public float ActualHits { get; set; }

    public float MinHitsPerGame { get; set; }

    public float MaxHitsPerGame { get; set; }

    public float AverageHitsPerGame { get; set; }

    public float HitsDeviation => (int)ExpectedHits == 0 ? 0 : (ActualHits - ExpectedHits) / ExpectedHits;
}
