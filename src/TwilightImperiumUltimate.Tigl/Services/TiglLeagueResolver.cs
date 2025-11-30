using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglLeagueResolver : ITiglLeagueResolver
{
    public TiglLeague Resolve(IGameReport gameReport)
    {
        return gameReport.League;
    }
}
