namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class AdminMainGrid
{
    private AdminMenuItem _selectedSegment = AdminMenuItem.Seasons;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private void ChangeSegment(AdminMenuItem item)
    {
        _selectedSegment = item;
        StateHasChanged();
    }
}
