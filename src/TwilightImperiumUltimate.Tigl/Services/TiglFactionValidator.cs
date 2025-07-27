using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglFactionValidator : ITiglFactionValidator
{
    public Task<Result<bool>> AllTiglFactionsAreValid(IGameReport gameReport)
    {
        var result = new Result<bool>();

        foreach (var playerResult in gameReport.PlayerResults)
        {
            try
            {
                _ = TiglFactionParser.ParseFaction(playerResult.Faction);
            }
            catch (ArgumentException ex)
            {
                result.WithError($"Cannot parse faction: {playerResult.Faction} for player {playerResult.DiscordTag}");
            }
        }

        return Task.FromResult(result);
    }
}
