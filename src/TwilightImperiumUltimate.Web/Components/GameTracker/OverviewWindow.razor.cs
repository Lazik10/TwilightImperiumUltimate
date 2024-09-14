using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class OverviewWindow
{
    private PlayersGrid _playerGrid = default!;
    private ObjectivesGrid _objectiveGrid = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IGameTrackerSettingsService GameTrackerSettingsService { get; set; } = default!;

    public async Task Refresh()
    {
        await _playerGrid.Refresh();
        await _objectiveGrid.Refresh();

        StateHasChanged();
    }
}