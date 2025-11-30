using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.DataAccess.Repositories;

public interface IRatingDecayRepository
{
    Task InsertRatingDecays(IReadOnlyCollection<TiglUser> inactivePlayers, TiglLeague league, int season, CancellationToken cancellationToken);
}
