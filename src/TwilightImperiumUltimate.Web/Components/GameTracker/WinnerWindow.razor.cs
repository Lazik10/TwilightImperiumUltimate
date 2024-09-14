using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class WinnerWindow
{
    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    private GameTrackerPlayerModel Winner { get; set; } = new GameTrackerPlayerModel();

    protected override void OnInitialized()
    {
        Winner = GameTrackerService.Players
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Initiative)
            .First();
    }
}