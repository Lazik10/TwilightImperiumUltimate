using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;
using TwilightImperiumUltimate.Draft.ValueObjects;

namespace TwilightImperiumUltimate.Draft.Drafts.ColorDraft;

public class ColorDraftService(
    IDbContextFactory<TwilightImperiumDbContext> dbContextFactory)
    : IColorDraftService
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _dbContextFactory = dbContextFactory;

    public async Task<FactionColorDraftResult> DraftColors(ColorDraftRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var factionColorImportances = await GetFactionColorImportances(cancellationToken);

        var filteredFactioncolorImportances = FilterFactionColorImportances(factionColorImportances, request.Factions);

        var result = PerformColorDraft(filteredFactioncolorImportances, request.Colors);

        return new FactionColorDraftResult(result);
    }

    private static Dictionary<FactionName, Dictionary<PlayerColor, int>> FilterFactionColorImportances(IReadOnlyDictionary<FactionName, Dictionary<PlayerColor, int>> factionColorImportances, IReadOnlyCollection<FactionName> requiredFactions)
    {
        return factionColorImportances
            .Where(pair => requiredFactions.Contains(pair.Key))
            .ToDictionary(pair => pair.Key, pair => new Dictionary<PlayerColor, int>(pair.Value));
    }

    private Dictionary<FactionName, PlayerColor> PerformColorDraft(
        Dictionary<FactionName, Dictionary<PlayerColor, int>> factionColorImportances,
        IReadOnlyCollection<PlayerColor> possibleColors)
    {
        var colors = possibleColors.ToList();

        Dictionary<FactionName, PlayerColor> allocation = [];

        while (factionColorImportances.Count > 0 && colors.Count > 0)
        {
            // Find the color that, if removed, maximizes the minimum utility for remaining factions
            var colorToRemove = colors.Select(color =>
            {
                var minUtility = factionColorImportances.Min(faction => faction.Value.GetValueOrDefault(color, 0));
                return (Color: color, MinUtility: minUtility);
            }).OrderByDescending(x => x.MinUtility).ThenBy(x => colors.IndexOf(x.Color)).First().Color;

            // Find the faction that values the colorToRemove the most
            var factionToAllocate = factionColorImportances.OrderByDescending(faction => faction.Value.GetValueOrDefault(colorToRemove, 0)).First().Key;

            // Allocate the color and remove the faction and color
            allocation[factionToAllocate] = colorToRemove;
            factionColorImportances.Remove(factionToAllocate);
            colors.Remove(colorToRemove);
        }

        return allocation;
    }

    private async Task<Dictionary<FactionName, Dictionary<PlayerColor, int>>> GetFactionColorImportances(CancellationToken cancellationToken)
    {
        await using var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);

        var factionColorMappings = new Dictionary<FactionName, Dictionary<PlayerColor, int>>();

        var factionColorImportances = await context.FactionColorImportances.ToListAsync(cancellationToken);

        foreach (var entity in factionColorImportances)
        {
            if (Enum.TryParse<FactionName>(entity.FactionName.ToString(), out var faction) &&
                Enum.TryParse<PlayerColor>(entity.Color.ToString(), out var color))
            {
                if (!factionColorMappings.TryGetValue(faction, out Dictionary<PlayerColor, int>? value))
                {
                    value = [];
                    factionColorMappings[faction] = value;
                }

                value[color] = entity.Importance;
            }
        }

        return factionColorMappings;
    }
}