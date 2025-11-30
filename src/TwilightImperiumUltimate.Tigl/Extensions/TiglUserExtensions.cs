using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;
using TwilightImperiumUltimate.Core.Entities.Tigl.Stats;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Extensions;

internal static class TiglUserExtensions
{
    public static PlayerStats GetPlayerGameMatchStats(this TiglUser user, int matchId, TiglLeague league)
    {
        var asyncPlayerMatchStats = user.GetPlayerGameAsyncMatchStats(matchId, league);
        var glickoPlayerMatchStats = user.GetPlayerGameGlickoMatchStats(matchId, league);
        var trueSkillPlayerMatchStats = user.GetPlayerGameTrueSkillMatchStats(matchId, league);

        return new PlayerStats(asyncPlayerMatchStats, glickoPlayerMatchStats, trueSkillPlayerMatchStats);
    }

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
        return user?.TrueSkillStats?.FirstOrDefault(x => x.League == league);
    }

    private static AsyncPlayerMatchStats? GetPlayerGameAsyncMatchStats(this TiglUser user, int matchId, TiglLeague league)
    {
        var stats = user?.AsyncStats?.FirstOrDefault(x => x.League == league);
        return stats?.MatchStats?.FirstOrDefault(x => x.MatchId == matchId);
    }

    private static GlickoPlayerMatchStats? GetPlayerGameGlickoMatchStats(this TiglUser user, int matchId, TiglLeague league)
    {
        var stats = user?.GlickoStats?.FirstOrDefault(x => x.League == league);
        return stats?.MatchStats?.FirstOrDefault(x => x.MatchId == matchId);
    }

    private static TrueSkillPlayerMatchStats? GetPlayerGameTrueSkillMatchStats(this TiglUser user, int matchId, TiglLeague league)
    {
        var stats = user?.TrueSkillStats?.FirstOrDefault(x => x.League == league);
        return stats?.MatchStats?.FirstOrDefault(x => x.MatchId == matchId);
    }
}
