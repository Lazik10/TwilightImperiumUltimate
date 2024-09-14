using TwilightImperiumUltimate.Web.Models.GameTracker;
using TwilightImperiumUltimate.Web.Services.GameTracker;

namespace TwilightImperiumUltimate.Web.Components.GameTracker;

public partial class PlayersGrid
{
    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Inject]
    private IGameTrackerService GameTrackerService { get; set; } = default!;

    public Task Refresh()
    {
        StateHasChanged();
        return Task.CompletedTask;
    }

    private List<GameTrackerPlayerModel> GetPlayers()
    {
        return GameTrackerService.Players
            .OrderByDescending(x => x.Score)
            .ThenBy(x => x.Initiative)
            .ToList();
    }
}