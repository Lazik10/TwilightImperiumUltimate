using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglResultValidator : ITiglResultValidator
{
    public Task<bool> ValidateResult(IGameReport gameReport)
    {
        ArgumentNullException.ThrowIfNull(gameReport);

        return Task.FromResult(gameReport.PlayerResults.Any(x => x.Score >= gameReport.Score));
    }
}
