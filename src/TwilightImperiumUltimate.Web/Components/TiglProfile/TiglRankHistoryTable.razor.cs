using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglRankHistoryTable
{
    [Parameter] public IReadOnlyList<RankHistoryDto> Ranks { get; set; } = [];
    private IReadOnlyList<RankHistoryRow> Rows => BuildRows();

    private string FormatDate(long timestamp) => timestamp <= 0 ? "-" : DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToString("yyyy-MM-dd");

    private IReadOnlyList<RankHistoryRow> BuildRows()
    {
        var snapshot = Ranks?.ToList() ?? [];
        return snapshot.Select((rank, index) => new RankHistoryRow(
            rank.Rank,
            rank.AchievedAt,
            index == snapshot.Count - 1 ? string.Empty : FormatDuration(snapshot[index + 1].AchievedAt, rank.AchievedAt)))
            .ToList();
    }

    private static string FormatDuration(long from, long to) { var days=(int)(DateTimeOffset.FromUnixTimeMilliseconds(to)-DateTimeOffset.FromUnixTimeMilliseconds(from)).TotalDays; return days < 1 ? "<24h" : $"{days} {(days == 1 ? "day" : "days")}"; }

    private sealed record RankHistoryRow(TiglRankName Rank, long AchievedAt, string Duration);
}
