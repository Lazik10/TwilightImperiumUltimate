namespace TwilightImperiumUltimate.Web.Models.Rankings;

public class TiglUserRanking
{
    public int Id { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public TiglRankName Rank { get; set; }

    public TiglPrestigeRank? Prestige { get; set; }

    public int PrestigeLevel { get; set; }

    public int FactionPrestigeRankCount { get; set; }

    public long LastAchievedAt { get; set; }

    public int GamesPlayed { get; set; }

    public bool HasPrestigeRank => Prestige is not null;
}
