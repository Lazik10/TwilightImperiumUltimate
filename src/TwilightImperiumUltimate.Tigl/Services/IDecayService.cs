namespace TwilightImperiumUltimate.Tigl.Services;

public interface IDecayService
{
    Task ApplyDecay(int season, CancellationToken cancellationToken = default);
}
