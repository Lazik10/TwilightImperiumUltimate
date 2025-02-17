namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;

public record AsyncPlayerVpStatsByVpGameDto
{
    public AsyncPlayerVpStatsByVpGameDto()
    {
    }

    public AsyncPlayerVpStatsByVpGameDto(
        int vpGameCategory,
        int totalVp,
        int maxPossibleVp,
        float averageVpPerGame,
        float averageVpPercentagePerGame,
        int games)
    {
        VpGameCategory = vpGameCategory;
        TotalVp = totalVp;
        MaxPossibleVp = maxPossibleVp;
        AverageVpPerGame = averageVpPerGame;
        AverageVpPercentagePerGame = averageVpPercentagePerGame;
        Games = games;
    }

    public int VpGameCategory { get; set; }

    public int TotalVp { get; set; }

    public int MaxPossibleVp { get; set; }

    public float VpPercentageOfTotal => MaxPossibleVp == 0 ? 0 : (float)TotalVp / MaxPossibleVp * 100;

    public float AverageVpPerGame { get; set; }

    public float AverageVpPercentagePerGame { get; set; }

    public int Games { get; set; }
}
