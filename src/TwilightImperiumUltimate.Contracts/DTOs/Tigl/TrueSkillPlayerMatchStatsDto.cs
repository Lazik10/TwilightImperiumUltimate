using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TrueSkillPlayerMatchStatsDto
{
    public int TiglUserId { get; set; }

    public long DiscordUserId { get; set; }

    public bool Winner { get; set; }

    public int Placement { get; set; }

    public int Score { get; set; }

    public TiglFactionName Faction { get; set; }

    public double ConservativeRatingNew => MuNew - (3 * SigmaNew);

    public double ConservativeRatingOld => MuOld - (3 * SigmaOld);

    public double ConservativeRatingChange => ConservativeRatingNew - ConservativeRatingOld;

    public double MuOld { get; set; }

    public double MuNew { get; set; }

    public double MuChange => MuNew - MuOld;

    public double SigmaOld { get; set; }

    public double SigmaNew { get; set; }

    public double SigmaChange => SigmaNew - SigmaOld;

    public double OpponentAvgRating { get; set; }

    public int Season { get; set; }

    public long StartTimestamp { get; set; }

    public long EndTimestamp { get; set; }

    public bool IsRankUpGame { get; set; }

    public bool ForcedReset { get; set; }

    public TiglRankName OldRank { get; set; }

    public TiglRankName NewRank { get; set; }
}
