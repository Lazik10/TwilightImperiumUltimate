using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class GlickoPlayerMatchStatsDto
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

    public double RdOld { get; set; }

    public double RdNew { get; set; }

    public double RdChange => RdNew - RdOld;

    public double VolatilityOld { get; set; }

    public double VolatilityNew { get; set; }

    public double VolatilityChange => VolatilityNew - VolatilityOld;

    public double OpponentAvgRating { get; set; }

    public int Season { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public bool IsRankUpGame { get; set; }

    public bool ForcedReset { get; set; }

    public TiglRankName OldRank { get; set; }

    public TiglRankName NewRank { get; set; }
}

