using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public interface ITiglResultValidator
{
    Task<bool> ValidateResult(IGameReport gameReport);
}
