using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Numbers;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglPrestigeHistoryTable
{
    [Parameter, EditorRequired] public TiglPlayerProfileDto Profile { get; set; } = default!;
    [Parameter] public IReadOnlyList<PrestigeRankHistoryDto> Prestiges { get; set; } = [];
    [Parameter, EditorRequired] public IPathProvider PathProvider { get; set; } = default!;
    private IReadOnlyList<PrestigeHistoryRow> Rows => BuildRows();

    private string FormatDate(long timestamp) => timestamp <= 0 ? "-" : DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToString("yyyy-MM-dd");
    private static TextColor GetPrestigeColor(PrestigeRankHistoryDto prestige) => prestige.PrestigeRank switch
    {
        TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
        TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
        TiglPrestigeRank.Tyrant => TextColor.Tyrant,
        _ when prestige.Faction != TiglFactionName.None => TextColor.Hero,
        _ => TextColor.White,
    };
    private static string GetPrestigeText(PrestigeRankHistoryDto prestige) => prestige.Level > 0 ? $"{prestige.PrestigeRank.GetDisplayName()} {prestige.Level.ToRomanNumeral()}" : prestige.PrestigeRank.GetDisplayName();

    private IReadOnlyList<PrestigeHistoryRow> BuildRows()
    {
        var prestiges = IsLegacyHistory()
            ? OrderLegacyPrestiges()
            : Prestiges.ToArray();
        return prestiges
            .Select((prestige, index) => new PrestigeHistoryRow(prestige, GetDuration(prestiges, index)))
            .ToList();
    }

    private bool IsLegacyHistory() => Prestiges.All(prestige => prestige.League == TiglLeague.ProphecyOfKings);

    private IReadOnlyList<PrestigeRankHistoryDto> OrderLegacyPrestiges()
    {
        var milestones = Prestiges
            .Where(prestige => prestige.PrestigeRank == TiglPrestigeRank.GalacticThreat)
            .OrderByDescending(prestige => prestige.Level)
            .ThenByDescending(prestige => prestige.AchievedAt)
            .ThenByDescending(prestige => prestige.Id)
            .ToList();
        var factionRanks = Prestiges
            .Where(prestige => prestige.Faction != TiglFactionName.None)
            .OrderByDescending(prestige => prestige.AchievedAt)
            .ThenByDescending(prestige => prestige.Id)
            .ToList();
        var orderedPrestiges = new List<PrestigeRankHistoryDto>(Prestiges.Count);

        foreach (var milestone in milestones)
        {
            orderedPrestiges.Add(milestone);
            orderedPrestiges.AddRange(factionRanks.Take(5));
            factionRanks.RemoveRange(0, Math.Min(5, factionRanks.Count));
        }

        orderedPrestiges.AddRange(factionRanks);
        orderedPrestiges.AddRange(Prestiges
            .Where(prestige => prestige.Faction == TiglFactionName.None && prestige.PrestigeRank != TiglPrestigeRank.GalacticThreat)
            .OrderByDescending(prestige => prestige.AchievedAt)
            .ThenByDescending(prestige => prestige.Id));
        return orderedPrestiges;
    }

    private static string GetDuration(IReadOnlyList<PrestigeRankHistoryDto> prestiges, int index)
    {
        if (index < 0 || index >= prestiges.Count - 1)
            return "-";

        var currentPrestige = prestiges[index];
        var previousPrestige = prestiges[index + 1];
        return FormatDuration(previousPrestige.AchievedAt, currentPrestige.AchievedAt);
    }

    private static string FormatDuration(long from, long to)
    {
        var duration = DateTimeOffset.FromUnixTimeMilliseconds(to) - DateTimeOffset.FromUnixTimeMilliseconds(from);
        if (duration <= TimeSpan.Zero)
            return "-";

        return duration.TotalDays < 1
            ? $"{(int)duration.TotalHours} h"
            : $"{(int)duration.TotalDays} d {duration.Hours} h";
    }

    private sealed record PrestigeHistoryRow(PrestigeRankHistoryDto Prestige, string Duration);
}
