using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class GameTrackerGrid
{
    private GameTrackerPhase _currentPhase;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    [Inject]
    private IGameTrackerSettingsService GameTrackerSettingsService { get; set; } = default!;

    public Task Refresh()
    {
        _currentPhase = GameTrackerService.CurrentPhase;
        StateHasChanged();
        return Task.CompletedTask;
    }

    protected override void OnInitialized()
    {
        _currentPhase = GameTrackerService.CurrentPhase;
    }
}