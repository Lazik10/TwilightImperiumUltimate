namespace TwilightImperiumUltimate.Web.Components.Async;

public partial class AsyncMenu
{
    [Parameter]
    public EventCallback<AsyncMenuItem> OnMenuItemClick { get; set; }
}