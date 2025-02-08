using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerTurnStats
{
    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    public AsyncPlayerTurnStatsDto TurnStats => StatType switch
    {
        PlayerStatisticsType.All => AsyncPlayerProfile.TurnStats.All,
        PlayerStatisticsType.Tigl => AsyncPlayerProfile.TurnStats.Tigl,
        PlayerStatisticsType.Custom => AsyncPlayerProfile.TurnStats.Custom,
        _ => AsyncPlayerProfile.TurnStats.All,
    };

    private string GetAverageTurn()
    {
        TimeSpan averageTurnTime = TimeSpan.FromMilliseconds(TurnStats.AverageTurnTime);

        List<string> parts = new List<string>();

        if (averageTurnTime.Days > 0)
            parts.Add($"{averageTurnTime.Days:D2} d");
        if (averageTurnTime.Hours > 0)
            parts.Add($"{averageTurnTime.Hours:D2} h");
        if (averageTurnTime.Minutes > 0)
            parts.Add($"{averageTurnTime.Minutes:D2} m");
        if (averageTurnTime.Seconds > 0)
            parts.Add($"{averageTurnTime.Seconds:D2} s");

        return parts.Count > 0 ? string.Join(" ", parts) : "0s";
    }

    private TextColor GetAverageTurnColor()
    {
        TimeSpan averageTurnTime = TimeSpan.FromMilliseconds(TurnStats.AverageTurnTime);

        if (averageTurnTime.Hours > 6)
            return TextColor.Red;
        if (averageTurnTime.Hours > 4)
            return TextColor.Orange;
        if (averageTurnTime.Hours > 2)
            return TextColor.Yellow;

        return TextColor.Green;
    }
}