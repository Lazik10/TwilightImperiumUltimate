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

    private string GetWinratePercentage() =>
        $"Win rate:\u2003\u2003{(AsyncPlayerProfile.Settings.ShowWinRates ?
            GameStats.WinRate.ToStringWithPrecisionAndPercentage(3)
            : Strings.AsyncPlayer_HiddenStat)}";

    private string GetGamesFinished() =>
        $"Finished games:\u2003\u2003{(AsyncPlayerProfile.Settings.ShowGames ?
            GameStats.Finished
            : Strings.AsyncPlayer_HiddenStat)}";

    private string ShowWinRate()
    {
        if (!AsyncPlayerProfile.Settings.ShowWinRates)
            return Strings.AsyncPlayer_HiddenStat;

        return GameStats.WinRate.ToStringWithPrecisionAndPercentage(3);
    }

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
