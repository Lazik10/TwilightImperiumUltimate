namespace TwilightImperiumUltimate.Web.Components.Shared;

public partial class InfoIcon
{
    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void HandleClick() => NavigationManager.NavigateTo(Pages.Pages.About);
}