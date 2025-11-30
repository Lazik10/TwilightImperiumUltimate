using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglMainGrid
{
    private TiglMenuItem _selectedSegment = TiglMenuItem.Leaderboard;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    [Parameter]
    public TiglMenuItem Segment { get; set; }

    [CascadingParameter(Name = "Category")]
    public string Category { get; set; } = string.Empty;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadSeasons();
    }

    protected override void OnParametersSet()
    {
        if (!string.IsNullOrEmpty(Category))
        {
            if (Category == "leaderboard")
                _selectedSegment = TiglMenuItem.Leaderboard;
            else if (Category == "statistics")
                _selectedSegment = TiglMenuItem.Statistics;
            else if (Category == "players")
                _selectedSegment = TiglMenuItem.Players;
            else if (Category == "games")
                _selectedSegment = TiglMenuItem.GameReports;
        }

        StateHasChanged();
    }

    private void ChangeSegment(TiglMenuItem segment)
    {
        _selectedSegment = segment;
        StateHasChanged();
    }

    private async Task LoadSeasons()
    {
        if (_seasons.Count > 0)
            return;

        var (seasonsResponse, seasonsStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
        if (seasonsStatus == HttpStatusCode.OK && seasonsResponse?.Data?.Items is not null)
        {
            var seasons = seasonsResponse.Data.Items;
            seasons.ToList().Add(new SeasonDto
            {
                SeasonNumber = -1,
                Name = "All Seasons",
            });

            _seasons = seasonsResponse.Data.Items;
        }
        else
        {
            _seasons = Array.Empty<SeasonDto>();
        }
    }

    private void RedirectToReportGameForm() => NavigationManager.NavigateTo(Pages.Pages.TiglReportGame);

    private void RedirectToRegisterPage() => NavigationManager.NavigateTo(Pages.Pages.TiglRegister);
}