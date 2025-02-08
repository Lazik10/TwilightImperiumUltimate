using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.TurnStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.VictoryPointsStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

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

    private string ShowGames(int games) => AsyncPlayerProfile.Settings.ShowGames ? games.ToString(CultureInfo.InvariantCulture) : Strings.AsyncPlayer_HiddenStat;
}