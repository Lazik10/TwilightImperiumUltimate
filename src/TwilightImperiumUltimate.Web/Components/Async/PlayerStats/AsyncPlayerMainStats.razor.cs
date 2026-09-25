using System.Globalization;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.MainStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Helpers.Numbers;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerMainStats
{
    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    /// <summary>
    /// Gets or sets content rendered between the profile summary and the main statistics table.
    /// </summary>
    [Parameter]
    public RenderFragment? SummaryContent { get; set; }

    public AsyncPlayerMainStatsDto GameStats => StatType switch
    {
        PlayerStatisticsType.All => AsyncPlayerProfile.GameStats.All,
        PlayerStatisticsType.Tigl => AsyncPlayerProfile.GameStats.Tigl,
        PlayerStatisticsType.Custom => AsyncPlayerProfile.GameStats.Custom,
        _ => AsyncPlayerProfile.GameStats.All,
    };

    private TextColor MainStatsTextColor => AsyncPlayerProfile.Settings.ExcludeFromAsyncStats || !AsyncPlayerProfile.Settings.ShowWinRates
        ? TextColor.White
        : GameStats.WinRate.GetWinrateColor();

    private string GetWinrateValue() => AsyncPlayerProfile.Settings.ShowWinRates
        ? GameStats.WinRate.ToStringWithPrecisionAndPercentage(1)
        : Strings.AsyncPlayer_HiddenStat;

    private string GetGamesFinishedValue() => AsyncPlayerProfile.Settings.ShowGames
        ? GameStats.Finished.ToString(CultureInfo.InvariantCulture)
        : Strings.AsyncPlayer_HiddenStat;

    private string ShowWins()
    {
        if (!AsyncPlayerProfile.Settings.ShowWinRates)
            return Strings.AsyncPlayer_HiddenStat;

        return GameStats.Wins.ToString(CultureInfo.InvariantCulture);
    }

    private string GetWinrateCssClass()
    {
        if (AsyncPlayerProfile.Settings.ExcludeFromAsyncStats || !AsyncPlayerProfile.Settings.ShowWinRates)
            return string.Empty;

        return GameStats.WinRate.GetWinrateColor() switch
        {
            TextColor.Green => "green",
            TextColor.Yellow => "yellow",
            TextColor.Orange => "orange",
            TextColor.Red => "red",
            _ => string.Empty,
        };
    }
}
