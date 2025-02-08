namespace TwilightImperiumUltimate.Contracts.DTOs.Async.AsyncStats;

public class AsyncFactionStatsByGameVpDto
{
    public int Games { get; set; }

    public int Wins { get; set; }

    public int Eliminations { get; set; }

    public int Vp { get; set; }

    public int MaxVp { get; set; }

    public float VpPercentage => MaxVp == 0 ? 0 : (float)Vp / MaxVp * 100;

    public float WinsPercentage => Games == 0 ? 0 : (float)Wins / Games * 100;

    public float EliminationsPercentage => Games == 0 ? 0 : (float)Eliminations / Games * 100;
}
