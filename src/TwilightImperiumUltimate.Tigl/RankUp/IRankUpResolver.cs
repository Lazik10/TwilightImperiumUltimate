using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Tigl.RankUp;

public interface IRankUpResolver
{
    Task ResolveRankUp(int gameId, TiglLeague league, CancellationToken cancellationToken);
}
