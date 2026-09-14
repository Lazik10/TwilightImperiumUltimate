namespace TwilightImperiumUltimate.Web.Components.Async;

public partial class AsyncMenu
{
    [Parameter]
    public AsyncMenuItem SelectedItem { get; set; } = AsyncMenuItem.Statistics;

    [Parameter]
    public EventCallback<AsyncMenuItem> OnMenuItemClick { get; set; }
}
