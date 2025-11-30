using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Tigl.Services;

public class DecayService(
    ITiglUserRepository tiglUserRepository,
    IRatingDecayRepository ratingDecayRepository)
    : IDecayService
{
    public async Task ApplyDecay(int season, CancellationToken cancellationToken = default)
    {
        var thundersEdgeDecayUserIds = await GetDecayUserIds(season, TiglLeague.ThundersEdge, cancellationToken);
        var fracturedDecayUserIds = await GetDecayUserIds(season, TiglLeague.Fractured, cancellationToken);

        await HandleDecayUpdate(thundersEdgeDecayUserIds, TiglLeague.ThundersEdge, season, cancellationToken);
        await HandleDecayUpdate(fracturedDecayUserIds, TiglLeague.Fractured, season, cancellationToken);
    }

    private async Task<HashSet<int>> GetDecayUserIds(int season, TiglLeague league, CancellationToken cancellationToken)
    {
        var allUserIds = await tiglUserRepository.GetAllTiglUserIds(cancellationToken);
        var activeUserIds = await tiglUserRepository.GetActiveUserIds(season, league, cancellationToken);
        var inactiveUserIds = allUserIds.Except(activeUserIds).ToHashSet();

        return inactiveUserIds;
    }

    private async Task HandleDecayUpdate(HashSet<int> decayUserIds, TiglLeague league, int season, CancellationToken cancellationToken)
    {
        var tiglUsers = await tiglUserRepository.GetUsersByIds(decayUserIds, cancellationToken);

        await ratingDecayRepository.InsertRatingDecays(tiglUsers, league, season, cancellationToken);
    }
}
