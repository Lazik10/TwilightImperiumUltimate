using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Contracts.DTOs.Tigl;

public class PlayerSeasonResultDto
{
    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public string TiglDiscordTag { get; set; } = string.Empty;

    public int Season { get; set; }

    public TiglLeague League { get; set; }

    public int GamesPlayed { get; set; }

    public double WinPercentage { get; set; }

    public double AsyncRating { get; set; }

    public double AussieScore { get; set; }

    public double GlickoRating { get; set; }

    public double GlickoRd { get; set; }

    public double GlickoVolatility { get; set; }

    public double TrueSkillMu { get; set; }

    public double TrueSkillSigma { get; set; }

    public double TrueSkillConservativeRating { get; set; }

    public IReadOnlyList<double> AsyncRatingDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> AussieScoreDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> GlickoRatingDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> GlickoRdDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> GlickoVolatilityDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> TrueSkillMuDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> TrueSkillSigmaDeltas { get; set; } = Array.Empty<double>();

    public IReadOnlyList<double> TrueSkillConservativeDeltas { get; set; } = Array.Empty<double>();
}
