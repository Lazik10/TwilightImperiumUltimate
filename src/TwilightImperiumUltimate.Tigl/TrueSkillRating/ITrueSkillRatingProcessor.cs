using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.TrueSkillRating;

public interface ITrueSkillRatingProcessor
{
    Task<bool> ProcessGameReport(IGameReport report, int gameId, CancellationToken cancellationToken);
}
