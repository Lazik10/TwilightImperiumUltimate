namespace TwilightImperiumUltimate.Tigl.RankUp;

public interface ITiglRankUpResolver
{
    Task ResolveRankUpAsync(int gameId, CancellationToken cancellationToken);
}
