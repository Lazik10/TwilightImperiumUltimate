using TwilightImperiumUltimate.Contracts.DTOs.Rankings;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglRankHistoryTable
{
    [Parameter]
    public IReadOnlyList<RankHistoryDto> Ranks { get; set; } = [];

    private IReadOnlyList<RankHistoryRow> Rows => BuildRows();

    private static string FormatDuration(long from, long to)
    {
        var days = (int)(DateTimeOffset.FromUnixTimeMilliseconds(to) - DateTimeOffset.FromUnixTimeMilliseconds(from)).TotalDays;
        if (days < 1)
            return "<24h";

        var unit = days == 1 ? "day" : "days";
        return $"{days} {unit}";
    }

    private string FormatDate(long timestamp) => timestamp <= 0 ? "-" : DateTimeOffset.FromUnixTimeMilliseconds(timestamp).ToString("yyyy-MM-dd");

    private List<RankHistoryRow> BuildRows()
    {
        var snapshot = Ranks?.ToList() ?? [];
        return snapshot.Select((rank, index) =>
        {
            var duration = index == snapshot.Count - 1
                ? string.Empty
                : FormatDuration(snapshot[index + 1].AchievedAt, rank.AchievedAt);

            return new RankHistoryRow(rank.Rank, rank.AchievedAt, duration);
        }).ToList();
    }

    private sealed record RankHistoryRow(TiglRankName Rank, long AchievedAt, string Duration);
}
