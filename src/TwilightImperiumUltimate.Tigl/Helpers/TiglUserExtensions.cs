using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;

namespace TwilightImperiumUltimate.Tigl.Helpers;

internal static class TiglUserExtensions
{
    public static AsyncStats? GetCorrectAsyncStats(this TiglUser user, TiglLeague league)
    {
        return user?.AsyncStats?.FirstOrDefault(x => x.League == league);
    }

    public static GlickoStats? GetCorrectGlickoStats(this TiglUser user, TiglLeague league)
    {
        return user?.GlickoStats?.FirstOrDefault(x => x.League == league);
    }

    public static TrueSkillStats? GetCorrectTrueSkillStats(this TiglUser user, TiglLeague league)
    {
        return user.TrueSkillStats?.FirstOrDefault(x => x.League == league);
    }
}
