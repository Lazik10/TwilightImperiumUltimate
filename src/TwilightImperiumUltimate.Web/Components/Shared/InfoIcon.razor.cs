namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class InfoIcon
{
    [Parameter]
    public int MaxWidth { get; set; } = 26;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void HandleClick() => NavigationManager.NavigateTo(Pages.Pages.About);
}