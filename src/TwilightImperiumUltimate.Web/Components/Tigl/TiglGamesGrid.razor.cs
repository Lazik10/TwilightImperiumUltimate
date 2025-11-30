using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglGamesGrid
{
    private int _selectedSeasonNumber;
    private List<MatchReportDto> _allStandard = new();
    private List<MatchReportDto> _allFractured = new();
    private bool _loading;

    private List<MatchReportDto> FilteredStandardGames { get; set; }

    private List<MatchReportDto> FilteredFracturedGames { get; set; }

    [CascadingParameter(Name = "Seasons")]
    public IReadOnlyCollection<SeasonDto> Seasons { get; set; } = Array.Empty<SeasonDto>();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        _selectedSeasonNumber = -1; // All by default
        await LoadGameReports();

        _loading = false;
    }

    private async Task LoadGameReports()
    {
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<MatchReportDto>>>(Paths.ApiPath_GameReports);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            var items = resp.Data.Items
                .OrderByDescending(m => m.EndTimestamp)
                .ToList();

            _allStandard = items.Where(m => m.League == TiglLeague.ThundersEdge || m.League == TiglLeague.ProphecyOfKings).ToList();
            _allFractured = items.Where(m => m.League == TiglLeague.Fractured).ToList();
            FilteredStandardGames = _allStandard;
            FilteredFracturedGames = _allFractured;
        }
    }

    private static string FormatEndDate(long endTs)
    {
        if (endTs <= 0)
            return "-";

        var dt = DateTimeOffset.FromUnixTimeMilliseconds(endTs).ToLocalTime().DateTime;
        return dt.ToString("dd/MM/yyyy HH:mm", System.Globalization.CultureInfo.InvariantCulture);
    }

    private void UpdateSelectedSeason(int seasonNumber)
    {
        _selectedSeasonNumber = seasonNumber;
        _loading = true;
        UpdateSelectedGames();
        _loading = false;
    }

    private void UpdateSelectedGames()
    {
        FilteredStandardGames = _allStandard.Where(x => x.Season == _selectedSeasonNumber).ToList();
        FilteredFracturedGames = _allFractured.Where(x => x.Season == _selectedSeasonNumber).ToList();
    }

    private void RedirectToGameDetail(int id) => NavigationManager.NavigateTo(Pages.Pages.TiglGameDetail + $"?id={id}");
}
