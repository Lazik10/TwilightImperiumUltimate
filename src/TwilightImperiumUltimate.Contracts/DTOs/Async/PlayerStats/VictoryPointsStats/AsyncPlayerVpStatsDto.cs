namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;

public record AsyncPlayerVpStatsDto
{
    public AsyncPlayerVpStatsDto()
    {
        AverageVp10VpGame = 0;
        Games10Vp = 0;
        AverageVp12VpGame = 0;
        Games12Vp = 0;
        AverageVp14VpGame = 0;
        Games14Vp = 0;
    }

    public AsyncPlayerVpStatsDto(
        float averageVp10VpGame,
        int games10vp,
        float averageVp12VpGame,
        int games12vp,
        float averageVp14VpGame,
        int games14vp)
    {
        AverageVp10VpGame = averageVp10VpGame;
        Games10Vp = games10vp;
        AverageVp12VpGame = averageVp12VpGame;
        Games12Vp = games12vp;
        AverageVp14VpGame = averageVp14VpGame;
        Games14Vp = games14vp;
    }

    public float AverageVp10VpGame { get; init; }

    public int Games10Vp { get; init; }

    public float AverageVp12VpGame { get; init; }

    public int Games12Vp { get; init; }

    public float AverageVp14VpGame { get; init; }

    public int Games14Vp { get; init; }
}
