using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.HitStats;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Helpers.Numbers;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerCombatStats
{
    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    public AsyncPlayerCombatStatsDto CombatStats => StatType switch
    {
        PlayerStatisticsType.All => AsyncPlayerProfile.CombatStats.Overall,
        PlayerStatisticsType.Tigl => AsyncPlayerProfile.CombatStats.Tigl,
        PlayerStatisticsType.Custom => AsyncPlayerProfile.CombatStats.Custom,
        _ => AsyncPlayerProfile.CombatStats.Overall,
    };

    private TextColor GetDiceDeviationTextColor()
    {
        return CombatStats.HitsDeviation switch
        {
            < 0 => TextColor.Red,
            > 0 => TextColor.Green,
            _ => TextColor.Yellow,
        };
    }

    private string GetHitDeviationText() => $"{CombatStats.HitsDeviation.ToStringWithPrecisionAndPercentage(3)}";
}