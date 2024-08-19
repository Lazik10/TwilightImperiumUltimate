using TwilightImperiumUltimate.Contracts.DTOs.Draft;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.FactionDraft;

public class FactionDraftService(
    ILogger<FactionDraftService> logger)
    : IFactionDraftService
{
    private static readonly Random _random = new();
    private readonly ILogger<FactionDraftService> _logger = logger;

    public Task<FactionDraftResult> DraftFactions(FactionDraftRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        _logger.LogInformation("Drafting {NumberOfFactions} factions for {NumberOfPlayers} players", request.NumberOfFactionsPerPlayer, request.NumberOfPlayers);

        foreach (var faction in request.Factions)
            _logger.LogInformation("Faction: {FactionName} - Status: {BanStatus}", faction.FactionName, faction.Banned);

        var availableFactions = request.Factions
            .Where(x => !x.Banned)
            .ToList();

        var result = Enumerable.Range(1, request.NumberOfPlayers)
            .Select(i => (i, PickFactionsForPlayer(availableFactions, request.NumberOfFactionsPerPlayer)))
            .ToDictionary();

        foreach (var draftResult in result)
        {
            var factionsString = string.Join(", ", draftResult.Value);
            _logger.LogInformation("Player {PlayerId} drafted {FactionCount} faction(s)", draftResult.Key, factionsString);
        }

        return Task.FromResult(new FactionDraftResult(result));
    }

    private static List<FactionName> PickFactionsForPlayer(List<DraftFactionDto> availableFactions, int numberOfFactions)
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
