using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Async.Statistics;

public partial class StatisticsMenu
{
    private IReadOnlyCollection<StatisticsMenuItem> _menuItems = [];

    [Parameter]
    public EventCallback<AsyncStatisticsTypeMenuItem> SelectedMenuItemChanged { get; set; }

    [Parameter]
    public AsyncStatisticsTypeMenuItem SelectedMenuItem { get; set; } = AsyncStatisticsTypeMenuItem.General;

    [Parameter]
    public int Width { get; set; } = 100;

    protected override void OnInitialized()
    {
        _menuItems = EnumExtensions.GetEnumValuesWithDisplayNames<AsyncStatisticsTypeMenuItem>()
            .Select(item => new StatisticsMenuItem(item.Key, item.Value))
            .ToList();
    }

    private Task SelectedItemItem(AsyncStatisticsTypeMenuItem menuItem)
    {
        return SelectedMenuItemChanged.InvokeAsync(menuItem);
    }

    private sealed record StatisticsMenuItem(AsyncStatisticsTypeMenuItem Value, string DisplayName);
}
