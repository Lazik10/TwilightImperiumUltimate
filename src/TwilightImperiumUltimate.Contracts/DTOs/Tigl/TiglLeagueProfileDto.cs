using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class TiglLeagueProfileDto
{
    public TiglLeague League { get; set; }

    public double AsyncRating { get; set; }

    public double AsyncAussieScore { get; set; }

    public double GlickoRating { get; set; }

    public double GlickoRd { get; set; }

    public double GlickoVolatility { get; set; }

    public double TrueSkillMu { get; set; }

    public double TrueSkillSigma { get; set; }

    public double TrueSkillConservative => TrueSkillMu - (3 * TrueSkillSigma);

    public TiglRankName CurrentRank { get; set; }

    public TiglRankName HighestRank { get; set; }

    public int GamesPlayed { get; set; }

    public List<AsyncPlayerMatchStatsDto> AsyncMatchHistory { get; set; } = new();

    public List<GlickoPlayerMatchStatsDto> GlickoMatchHistory { get; set; } = new();

    public List<TrueSkillPlayerMatchStatsDto> TrueSkillMatchHistory { get; set; } = new();
}
