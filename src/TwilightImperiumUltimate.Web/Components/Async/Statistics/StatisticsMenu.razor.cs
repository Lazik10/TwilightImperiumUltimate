using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsMenu
{
    private List<AsyncStatisticsTypeMenuItem> _menuItems = new List<AsyncStatisticsTypeMenuItem>();

    [Parameter]
    public EventCallback<AsyncStatisticsTypeMenuItem> SelectedMenuITem { get; set; }

    [Parameter]
    public int Width { get; set; } = 100;

    [Inject]
    private IAsyncGamesProvider AsyncGameProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        _menuItems = GetMenuItems();
    }

    private List<AsyncStatisticsTypeMenuItem> GetMenuItems() => Enum.GetValues<AsyncStatisticsTypeMenuItem>().ToList();

    private void SelectedItemItem(AsyncStatisticsTypeMenuItem menuItem)
    {
        SelectedMenuITem.InvokeAsync(menuItem);
        StateHasChanged();
    }
}
