using Microsoft.Extensions.DependencyInjection;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.DataAccess.Repositories;

namespace TwilightImperiumUltimate.Tigl.RankUp;

internal class TiglRankUpResolver(
    IServiceProvider serviceProvider,
    ITiglRepository tiglRepository)
    : IRankUpResolver
{
    public async Task ResolveRankUp(int gameId, TiglLeague league, CancellationToken cancellationToken)
    {
        if (league == TiglLeague.Test)
            return;

        var enablePoKRankUp = await tiglRepository.GetTiglParameter(TiglParameterName.RankingUpProphecyOfKings, cancellationToken);
        if (enablePoKRankUp is not null && !enablePoKRankUp.Enabled && league == TiglLeague.ProphecyOfKings)
            return;

        var rankUpResolver = serviceProvider.GetRequiredKeyedService<ITiglRankUpResolver>(league);
        await rankUpResolver.ResolveRankUpAsync(gameId, cancellationToken);
    }
}
