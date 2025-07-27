using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglFactionValidator
{
    Task<Result<bool>> AllTiglFactionsAreValid(IGameReport gameReport);
}
