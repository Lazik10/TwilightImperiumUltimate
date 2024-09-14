using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class ObjectivesWindow
{
    private GameTrackerWindow _window = GameTrackerWindow.PublicObjectives;

    [Inject]
    private IObjectiveCardTracker ObjectiveCardTracker { get; set; } = default!;

    private void UpdateWindow(GameTrackerWindow window)
    {
        _window = window;
        StateHasChanged();
    }
}