using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Web.Components.TiglAdmin;

public partial class AdminMainGrid
{
    private AdminMenuItem _selectedSegment = AdminMenuItem.Seasons;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadSeasonsAsync();
    }

    private async Task LoadSeasonsAsync()
    {
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
            _seasons = resp.Data.Items.ToList();
        else
            _seasons = Array.Empty<SeasonDto>();
    }

    private void ChangeSegment(AdminMenuItem item)
    {
        _selectedSegment = item;
        StateHasChanged();
    }

    private void OnEditSeason(int seasonNumber)
    {
        NavigationManager.NavigateTo($"/tigl/season-edit?season={seasonNumber}");
    }
}
