using FluentResults;
using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Report;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Tigl.Helpers;

namespace TwilightImperiumUltimate.Tigl.Services;

public class TiglFactionValidator : ITiglFactionValidator
{
    public Task<Result<bool>> AllTiglFactionsAreValid(IGameReport gameReport)
    {
        var result = new Result<bool>();
        var validFactions = new List<TiglFactionName>();
        var allParsedSuccessfully = true;

        foreach (var playerResult in gameReport.PlayerResults)
        {
            try
            {
                var faction = TiglFactionParser.ParseFaction(playerResult.Faction);
                validFactions.Add(faction);
            }
            catch (ArgumentException ex)
            {
                allParsedSuccessfully = false;
                result.WithError($"Cannot parse faction: {playerResult.Faction} for player {playerResult.DiscordTag}.\n Exception: {ex}");
            }
        }

        if (gameReport.League == TiglLeague.ThundersEdge)
        {
            var valid = validFactions.All(x => x <= TiglFactionName.TheRalNelConsortium);

            if (!allParsedSuccessfully)
            {
                result.WithError($"Unable to parse one of the factions found for Official Thunder's Edge league. Factions: {string.Join(", ", gameReport.PlayerResults.Select(x => x.Faction))}");
                return Task.FromResult(result);
            }

            if (!valid)
            {
                result.WithError($"One or more factions are invalid for Official Thunder's Edge league! Factions: {string.Join(", ", gameReport.PlayerResults.Select(x => x.Faction))}, if you forgot to setup the game as Fractured league, please report the game manually!");
                return Task.FromResult(result);
            }
        }

        return Task.FromResult(result);
    }
}
