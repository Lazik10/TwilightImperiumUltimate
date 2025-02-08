namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public record AsyncEliminationsPlayerDto(
    int Id,
    string UserName,
    int Eliminations,
    int Games)
{
    public float EliminationsPercentage => Games == 0 ? 0 : (float)Eliminations / Games * 100;
}
