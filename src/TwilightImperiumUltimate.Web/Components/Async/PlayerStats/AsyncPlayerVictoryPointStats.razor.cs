using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Helpers.Numbers;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerVictoryPointStats
{
    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    public AsyncPlayerVpStatsDto VpStats => StatType switch
    {
        PlayerStatisticsType.All => AsyncPlayerProfile.VpStats.All,
        PlayerStatisticsType.Tigl => AsyncPlayerProfile.VpStats.Tigl,
        PlayerStatisticsType.Custom => AsyncPlayerProfile.VpStats.Custom,
        _ => AsyncPlayerProfile.VpStats.All,
    };

    private static string GetAverageVpCssClass(float averageVp) => ToCssClass(averageVp.GetAverageVpColor());

    private static string GetAverageVpPercentageCssClass(float averageVpPercentage) => ToCssClass(averageVpPercentage.GetAverageVpPercentageColor());

    private static string ToCssClass(TextColor color) => color switch
    {
        TextColor.Green => "green",
        TextColor.Yellow => "yellow",
        TextColor.Orange => "orange",
        TextColor.Red => "red",
        _ => string.Empty,
    };

    private string ShowGames(int games) => AsyncPlayerProfile.Settings.ShowGames ? games.ToString(CultureInfo.InvariantCulture) : Strings.AsyncPlayer_HiddenStat;
}
