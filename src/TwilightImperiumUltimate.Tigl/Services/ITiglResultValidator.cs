using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglResultValidator
{
    Task<Result> ValidateResult(IGameReport gameReport);
}
