namespace TwilightImperiumUltimate.Web.Components.SliceGenerators;

public partial class SliceQuickMenu
{
    [Parameter]
    public EventCallback<IconType> OnMenuIconClick { get; set; }

    private async Task HandleMenuIconClick(IconType iconType)
    {
        await OnMenuIconClick.InvokeAsync(iconType);
    }
}