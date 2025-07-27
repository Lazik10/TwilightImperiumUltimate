using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.AsyncRating;

public interface IAsyncRatingProcessor
{
    Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken);
}
