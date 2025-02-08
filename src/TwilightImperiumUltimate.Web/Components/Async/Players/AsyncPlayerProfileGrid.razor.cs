using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Components.Async.Players;

public partial class AsyncPlayerProfileGrid
{
    public PlayerStatisticsType CurentStatisticsType { get; set; }

    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    private void OnEnumChanged(PlayerStatisticsType statisticsType)
    {
        CurentStatisticsType = statisticsType;
        StateHasChanged();
    }
}