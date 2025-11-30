using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.DataAccess.DTOs;

public class RankingsRow
{
    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public TiglLeague League { get; set; }

    public TiglRankName HighestRank { get; set; }

    public long LastAchievedAt { get; set; }

    public TiglPrestigeRank? HighestPrestigeRank { get; set; }

    public int HighestPrestigeRankLevel { get; set; }

    public int FactionPrestigeRankCount { get; set; }

    public int GamesPlayed { get; set; }
}
