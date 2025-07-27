using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Glicko2Rating;

public interface IGlickoRatingProcessor
{
    Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken);
}
