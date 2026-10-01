using Microsoft.AspNetCore.Components.Web;
using TwilightImperiumUltimate.Contracts.DTOs.Async.PlayerStats.Opponents;
using TwilightImperiumUltimate.Contracts.DTOs.Async.Responses;

namespace TwilightImperiumUltimate.Web.Components.Async.PlayerStats;

public partial class AsyncPlayerOpponentsStats
{
    [CascadingParameter(Name = "AsyncPlayerProfile")]
    public AsyncPlayerProfileSummaryStatsDto AsyncPlayerProfile { get; set; } = default!;

    [CascadingParameter(Name = "AsyncPlayerStatisticsType")]
    public PlayerStatisticsType StatType { get; set; }

    [Inject]
    public NavigationManager NavigationManager { get; set; } = default!;

    private IReadOnlyCollection<AsyncPlayerOpponentInfoDto> GetOpponentsByStatType()
    {
        return StatType switch
        {
            PlayerStatisticsType.All => AsyncPlayerProfile.Opponents.All,
            PlayerStatisticsType.Tigl => AsyncPlayerProfile.Opponents.Tigl,
            PlayerStatisticsType.Custom => AsyncPlayerProfile.Opponents.Custom,
            _ => AsyncPlayerProfile.Opponents.All,
        };
    }

    private void ViewProfile(int id) => NavigationManager.NavigateTo($"{Pages.Pages.AsyncProfile}?playerId={id}");

    private void OnOpponentKeyDown(KeyboardEventArgs e, int id)
    {
        if (e.Key is "Enter" or " ")
            ViewProfile(id);
    }
}
