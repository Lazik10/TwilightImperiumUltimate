using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.Helpers;

public class PlayerStats
{
    public PlayerStats(AsyncPlayerMatchStats? asyncStats, GlickoPlayerMatchStats? glickoStats, TrueSkillPlayerMatchStats? trueSkillStats)
    {
        AsyncStats = asyncStats;
        GlickoStats = glickoStats;
        TrueSkillStats = trueSkillStats;
    }

    public AsyncPlayerMatchStats? AsyncStats { get; set; }

    public GlickoPlayerMatchStats? GlickoStats { get; set; }

    public TrueSkillPlayerMatchStats? TrueSkillStats { get; set; }

    public bool Valid => AsyncStats is not null && GlickoStats is not null && TrueSkillStats is not null;
}
