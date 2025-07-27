using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public interface IAsyncPlayerMatchStatsService
{
    Task<List<AsyncPlayerMatchStats>> InitializePlayerMatchStats(IGameReport report, int gameId, IReadOnlyCollection<TiglUser> players, TiglLeague league);
}
