using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public interface IAsyncRatingCalculatorService
{
    Task UpdatePlayerMatchStats(IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, int season);

    Task UpdatePlayerRatings(IReadOnlyCollection<TiglUser> players, IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, TiglLeague league);
}
