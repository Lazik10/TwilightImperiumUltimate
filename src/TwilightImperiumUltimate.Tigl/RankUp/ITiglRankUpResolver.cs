namespace TwilightImperiumUltimate.Tigl.RankUp;

public interface ITiglRankUpResolver
{
    public Task ResolveRankUpAsync(int gameId, CancellationToken cancellationToken);
}
