using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Core.Entities.Tigl;
using TwilightImperiumUltimate.Core.Entities.Tigl.History;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglRankUpResolver
{
    public Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<AsyncPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken);

    public Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<GlickoPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken);

    public Task ResolveRankUpAsync(IReadOnlyCollection<TiglUser> tiglUsers, IReadOnlyCollection<TrueSkillPlayerMatchStats> matchStats, TiglLeague league, CancellationToken cancellationToken);
}
