using TwilightImperiumUltimate.Web.Services.Async;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsMenu
{
    private List<AsyncStatisticsTypeMenuItem> _menuItems = new List<AsyncStatisticsTypeMenuItem>();
    private AsyncStatisticsTypeMenuItem _selectedMenuItem = AsyncStatisticsTypeMenuItem.General;

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
        _selectedMenuItem = menuItem;
        SelectedMenuITem.InvokeAsync(menuItem);
        StateHasChanged();
    }

    private string GetMenuItemColor(AsyncStatisticsTypeMenuItem menuItem)
    {
        var color = menuItem == _selectedMenuItem ? "lawngreen" : "white";
        return $"color: {color}";
    }
}