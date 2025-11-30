using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Core.Entities.Tigl;

public class PlayerSeasonResult
{
    public int Id { get; set; }

    public int Season { get; set; }

    public int TiglUserId { get; set; }

    public string TiglUserName { get; set; } = string.Empty;

    public string TiglDiscordTag { get; set; } = string.Empty;

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

    public bool IsActive { get; set; }

    public long CreatedAt { get; set; }
}
