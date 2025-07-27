using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.History;

public class GlickoPlayerMatchStats
{
    public int Id { get; set; }

    public int GlickoStatsId { get; set; }

    public GlickoStats? GlickoStats { get; set; }

    public int MatchId { get; set; }

    public MatchReport? Match { get; set; }

    public long DiscordUserId { get; set; }

    public int Score { get; set; }

    public bool Winner { get; set; }

    public int Placement { get; set; }

    public TiglFactionName Faction { get; set; }

    public double Performance { get; set; }

    public double RatingOld { get; set; }

    public double RatingNew { get; set; }

    public double RatingChange => RatingNew - RatingOld;

    public double RdOld { get; set; }

    public double RdNew { get; set; }

    public double RDChange => RatingNew - RatingOld;

    public double VolatilityOld { get; set; }

    public double VolatilityNew { get; set; }

    public double VolatilityChange => VolatilityNew - VolatilityOld;

    public double OpponentAvgRating { get; set; }

    public int Season { get; set; }

    public long Timestmap { get; set; }

    public bool IsRankUpGame { get; set; }

    public TiglAsyncRank OldRank { get; set; }

    public TiglAsyncRank NewRank { get; set; }
}
