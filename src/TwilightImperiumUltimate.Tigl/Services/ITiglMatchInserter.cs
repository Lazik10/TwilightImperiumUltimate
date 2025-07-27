using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Core.Entities.Tigl;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglMatchInserter
{
    Task<IResult<MatchReport>> InsertGameReport(GameReport gameReport, CancellationToken cancellationToken);
}
