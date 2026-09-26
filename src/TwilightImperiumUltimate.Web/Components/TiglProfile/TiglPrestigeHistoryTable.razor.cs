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
    private static TextColor GetPrestigeColor(TiglPrestigeRank rank) => rank switch
    {
        TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
        TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
        TiglPrestigeRank.Tyrant => TextColor.Tyrant,
        _ => TextColor.White,
    };
    private static string GetPrestigeText(PrestigeRankHistoryDto prestige) => prestige.Level > 0 ? $"{prestige.PrestigeRank.GetDisplayName()} {prestige.Level.ToRomanNumeral()}" : prestige.PrestigeRank.GetDisplayName();

    private IReadOnlyList<PrestigeHistoryRow> BuildRows()
    {
        var prestiges = Prestiges.ToArray();
        return prestiges
            .Select((prestige, index) => new PrestigeHistoryRow(prestige, GetDuration(prestiges, index)))
            .ToList();
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
