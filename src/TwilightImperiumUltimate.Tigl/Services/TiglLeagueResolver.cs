using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglLeagueResolver : ITiglLeagueResolver
{
    public TiglLeague Resolve(IGameReport gameReport)
    {
        var factions = gameReport.PlayerResults.Select(x => TiglFactionParser.ParseFaction(x.Faction)).ToList();

        return factions switch
        {
            var f when f.All(x => TiglFactionsProvider.TiglFactions.Contains(x))
                => TiglLeague.Tigl,

            var f when f.All(x => TiglFactionsProvider.DiscordantStarsFactions.Contains(x))
                => TiglLeague.Homebrew,

            _ => TiglLeague.Open,
        };
    }
}
