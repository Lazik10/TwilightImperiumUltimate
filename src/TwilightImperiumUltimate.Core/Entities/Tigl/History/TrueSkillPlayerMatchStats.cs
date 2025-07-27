using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.Core.Entities.Tigl.History;

public class TrueSkillPlayerMatchStats
{
    public int Id { get; set; }

    public int TrueSkillStatsId { get; set; }

    public TrueSkillStats? TrueSkillStats { get; set; }

    public int MatchId { get; set; }

    public MatchReport? Match { get; set; }

    public long DiscordUserId { get; set; }

    public int Score { get; set; }

    public bool Winner { get; set; }

    public int Placement { get; set; }

    public TiglFactionName Faction { get; set; }

    public double Performance { get; set; }

    public double MuOld { get; set; }

    public double MuNew { get; set; }

    public double MuChange => MuNew - MuOld;

    public double SigmaOld { get; set; }

    public double SigmaNew { get; set; }

    public double SigmaChange => SigmaNew - SigmaOld;

    public double OpponentAvgRating { get; set; }

    public int Season { get; set; }

    public long Timestamp { get; set; }

    public bool IsRankUpGame { get; set; }

    public TiglAsyncRank OldRank { get; set; }

    public TiglAsyncRank NewRank { get; set; }
}
