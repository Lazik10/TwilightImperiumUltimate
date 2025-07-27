using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public interface IGlickoRatingCalculatorService
{
    Task UpdatePlayerMatchStats(IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, int season);

    Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, TiglLeague league);
}
