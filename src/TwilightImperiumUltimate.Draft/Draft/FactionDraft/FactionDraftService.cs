using Microsoft.Extensions.Logging;
using TwilightImperiumUltimate.API.Models.Factions;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Models.Factions;

namespace TwilightImperiumUltimate.Draft.Draft.FactionDraft;

public class FactionDraftService : IFactionDraftService
{
    private static readonly Random _random = new Random();
    private readonly ILogger<FactionDraftService> _logger;

    public FactionDraftService(ILogger<FactionDraftService> logger)
    {
        _logger = logger;
    }

    public async Task<List<DraftResult>> DraftFactionsAsync(FactionDraftRequest draftRequest)
    {
        _logger.LogInformation("Drafting {NumberOfFactions} factions for {NumberOfPlayers} players", draftRequest.NumberOfFactionsPerPlayer, draftRequest.NumberOfPlayers);

        foreach (var faction in draftRequest.Factions)
            _logger.LogInformation("Faction: {FactionName} - Status: {BanStatus}", faction.FactionName, faction.Banned);

        var availableFactions = draftRequest.Factions
            .Where(x => !x.Banned)
            .ToList();

        var result = Enumerable.Range(0, draftRequest.NumberOfPlayers)
            .Select(i => new DraftResult
            {
                PlayerId = i + 1,
                Factions = PickFactions(availableFactions, draftRequest.NumberOfFactionsPerPlayer),
            }).ToList();

        foreach (var draftResult in result)
        {
            var factionsString = string.Join(", ", draftResult.Factions);
            _logger.LogInformation("Player {PlayerId} drafted {FactionCount} faction(s)", draftResult.PlayerId, factionsString);
        }

        return await Task.FromResult(result);
    }

    private static List<FactionName> PickFactions(List<FactionModel> availableFactions, int numberOfFactions)
    {
        var pickedRandomFactions = availableFactions
            .OrderBy(x => _random.Next())
            .Take(numberOfFactions)
            .ToList();

        foreach (var faction in pickedRandomFactions)
            availableFactions.Remove(faction);

        return pickedRandomFactions.Select(x => x.FactionName).ToList();
    }
}
