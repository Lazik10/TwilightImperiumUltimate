using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public interface ITrueSkillRatingCalculatorService
{
    Task UpdatePlayerMatchStats(IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, int season);

    Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, TiglLeague league);
}
