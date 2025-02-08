namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncCombatPlayerDto(
    int Id,
    string UserName,
    int Games,
    float Hits,
    float ExpectedHits,
    float MaxHitPerGame)
{
    public float HitsDeviation => (int)ExpectedHits == 0 ? 0 : (Hits - ExpectedHits) / ExpectedHits;

    public float AverageHits => Games == 0 ? 0 : Hits / Games;
}
