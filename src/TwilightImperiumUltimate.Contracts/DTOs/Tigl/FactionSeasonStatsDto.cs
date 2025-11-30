using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class FactionSeasonStatsDto
{
    public TiglFactionName Faction { get; set; }

    public int GamesPlayed { get; set; }

    public int Wins { get; set; }

    public double WinRate { get; set; }

    public int TotalScore { get; set; }

    public double AverageScore { get; set; }
}
