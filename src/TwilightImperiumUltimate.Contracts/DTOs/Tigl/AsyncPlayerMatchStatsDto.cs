using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class AsyncPlayerMatchStatsDto
{
    public int TiglUserId { get; set; }

    public long DiscordUserId { get; set; }

    public bool Winner { get; set; }

    public int Placement { get; set; }

    public int Score { get; set; }

    public TiglFactionName Faction { get; set; }

    public double RatingOld { get; set; }

    public double RatingNew { get; set; }

    public double RatingChange => RatingNew - RatingOld;

    public double AussieScoreOld { get; set; }

    public double AussieScoreNew { get; set; }

    public double AussieScoreChange => AussieScoreNew - AussieScoreOld;

    public double OpponentAvgRating { get; set; }

    public int Season { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public bool IsRankUpGame { get; set; }

    public bool ForcedReset { get; set; }

    public TiglRankName OldRank { get; set; }

    public TiglRankName NewRank { get; set; }
}
