using Microsoft.EntityFrameworkCore;
using TwilightImperiumUltimate.Core.Enums.Game;
using TwilightImperiumUltimate.Core.Enums.Races;
using TwilightImperiumUltimate.Core.Models.Factions;
using TwilightImperiumUltimate.DataAccess.DbContexts.TwilightImperium;

namespace TwilightImperiumUltimate.Draft.Draft.ColorDraft;

public class ColorDraftService : IColorDraftService
{
    private readonly IDbContextFactory<TwilightImperiumDbContext> _dbContextFactory;

    public ColorDraftService(IDbContextFactory<TwilightImperiumDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<IReadOnlyList<FactionColorDraftResult>> DraftColorsAsync(ColorDraftRequest draftRequest)
    {
        var factionColorImportances = GetFactionColorImportances();

        var filteredFactioncolorImportances = FilterFactioncolorImportances(factionColorImportances, draftRequest.Factions);

        var result = PerformColorDraft(filteredFactioncolorImportances, draftRequest.Colors);

        return await ConvertToFactionColorDraftResult(result);
    }

    private static async Task<IReadOnlyList<FactionColorDraftResult>> ConvertToFactionColorDraftResult(Dictionary<FactionName, PlayerColor> result)
    {
        var colorDraftResults = new List<FactionColorDraftResult>();

        foreach (var (faction, color) in result)
        {
            var factionColorDraftResult = new FactionColorDraftResult
            {
                FactionName = faction,
                Color = color,
            };

            colorDraftResults.Add(factionColorDraftResult);
        }

        return await Task.FromResult<IReadOnlyList<FactionColorDraftResult>>(colorDraftResults);
    }

    private Dictionary<FactionName, Dictionary<PlayerColor, int>> FilterFactioncolorImportances(Dictionary<FactionName, Dictionary<PlayerColor, int>> factionColorImportances, IReadOnlyCollection<FactionName> factions)
    {
        return factionColorImportances
            .Where(pair => factions.Contains(pair.Key))
            .ToDictionary(pair => pair.Key, pair => new Dictionary<PlayerColor, int>(pair.Value));
    }

    private Dictionary<FactionName, PlayerColor> PerformColorDraft(
        Dictionary<FactionName, Dictionary<PlayerColor, int>> factionColorImportances,
        IReadOnlyCollection<PlayerColor> possibleColors)
    {
        var colors = possibleColors.ToList();

        Dictionary<FactionName, PlayerColor> allocation = new();

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

    private Dictionary<FactionName, Dictionary<PlayerColor, int>> GetFactionColorImportances()
    {
        using var context = _dbContextFactory.CreateDbContext();

        var factionColorMappings = new Dictionary<FactionName, Dictionary<PlayerColor, int>>();

        var factionColorImportances = context.FactionColorImportances.ToList();

        foreach (var entity in factionColorImportances)
        {
            if (Enum.TryParse<FactionName>(entity.FactionName.ToString(), out var faction) &&
                Enum.TryParse<PlayerColor>(entity.Color.ToString(), out var color))
            {
                if (!factionColorMappings.TryGetValue(faction, out Dictionary<PlayerColor, int>? value))
                {
                    value = new Dictionary<PlayerColor, int>();
                    factionColorMappings[faction] = value;
                }

                value[color] = entity.Importance;
            }
        }

        return factionColorMappings;
    }
}