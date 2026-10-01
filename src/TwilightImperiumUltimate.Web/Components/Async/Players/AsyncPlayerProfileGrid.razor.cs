using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayerProfileGrid
{
    public PlayerStatisticsType CurentStatisticsType { get; set; } = PlayerStatisticsType.All;

    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    private static List<KeyValuePair<PlayerStatisticsType, string>> GetStatisticsTypeOptions() => Enum.GetValues<PlayerStatisticsType>()
        .Select(value => new KeyValuePair<PlayerStatisticsType, string>(value, value.GetDisplayName()))
        .ToList();

    private void OnEnumChanged(PlayerStatisticsType statisticsType)
    {
        CurentStatisticsType = statisticsType;
    }
}
