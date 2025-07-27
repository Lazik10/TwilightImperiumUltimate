using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglMatchUsersValidator
{
    Task<bool> AllTiglUsersExists(IGameReport gameReport, CancellationToken cancellationToken);

    Task<Result<bool>> RegisterNewTiglUsers(IGameReport gameReport, CancellationToken cancellationToken);
}
