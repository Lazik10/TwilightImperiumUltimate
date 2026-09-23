using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglPrestigeHistoryTable
{
    [Parameter, EditorRequired] public TiglPlayerProfileDto Profile { get; set; } = default!;
    [Parameter] public IReadOnlyList<PrestigeRankHistoryDto> Prestiges { get; set; } = [];
    [Parameter, EditorRequired] public IPathProvider PathProvider { get; set; } = default!;
    private string FormatDate(long timestamp) => timestamp <= 0 ? "-" : DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToString("yyyy-MM-dd");
    private static TextColor GetPrestigeColor(TiglPrestigeRank rank) => rank switch
    {
        TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
        TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
        TiglPrestigeRank.Tyrant => TextColor.Tyrant,
        _ => TextColor.White,
    };
    private string GetDuration(int index) => index == Prestiges.Count - 1 ? string.Empty : FormatDuration(Prestiges[index + 1].AchievedAt, Prestiges[index].AchievedAt);
    private static string FormatDuration(long from, long to) { var days=(int)(DateTimeOffset.FromUnixTimeMilliseconds(to)-DateTimeOffset.FromUnixTimeMilliseconds(from)).TotalDays; return days < 1 ? "<24h" : $"{days} {(days == 1 ? "day" : "days")}"; }
}
