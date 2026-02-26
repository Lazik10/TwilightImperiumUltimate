using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglProfileFactionStatsDto
{
    public TiglFactionName Faction { get; set; }

    public TiglLeague League { get; set; }

    public int Games { get; set; }

    public int Wins { get; set; }

    public int MinVp { get; set; }

    public int MaxVp { get; set; }

    public int TotalScoredVp { get; set; }

    public int MaxPossibleVp { get; set; }

    public float WinRate => Games == 0 ? 0 : (float)Wins / Games * 100;

    public float AverageVp => Games == 0 ? 0 : (float)TotalScoredVp / Games;

    public float AverageVpPercentage => MaxPossibleVp == 0 ? 0 : (float)TotalScoredVp / MaxPossibleVp * 100;
}
