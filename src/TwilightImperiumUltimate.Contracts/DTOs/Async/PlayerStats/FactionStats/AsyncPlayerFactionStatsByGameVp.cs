namespace TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.FactionStats;

public record AsyncPlayerFactionStatsByGameVp
{
    public AsyncPlayerFactionStatsByGameVp()
    {
        Games = 0;
        Wins = 0;
        MaxPossibleVp = 0;
        TotalScoredVp = 0;
        MinVp = 0;
        MaxVp = 0;
    }

    public AsyncPlayerFactionStatsByGameVp(int games, int wins, int maxPossibleVp, int totalScoredVp, int minVp, int maxVp)
    {
        Games = games;
        Wins = wins;
        MaxPossibleVp = maxPossibleVp;
        TotalScoredVp = totalScoredVp;
        MinVp = minVp;
        MaxVp = maxVp;
    }

    public int Games { get; set; }

    public int Wins { get; set; }

    public int Eliminations { get; set; }

    public int MaxPossibleVp { get; set; }

    public int TotalScoredVp { get; set; }

    public int MinVp { get; set; }

    public int MaxVp { get; set; }

    public float WinRate => Games == 0 ? 0 : (float)Wins / Games * 100;

    public float AverageVp => Games == 0 ? 0 : (float)TotalScoredVp / Games;

    public float AverageVpPercentage => MaxPossibleVp == 0 ? 0 : (float)TotalScoredVp / MaxPossibleVp * 100;
}
