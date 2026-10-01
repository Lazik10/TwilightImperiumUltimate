using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglGamesGrid
{
    private bool _areSeasonFiltersVisible;
    private int _defaultSeasonNumber;
    private int _selectedSeasonNumber;
    private TiglLeague _selectedLeague = TiglLeague.ProphecyOfKings;
    private List<MatchReportDto> _allStandard = new();
    private List<MatchReportDto> _allFractured = new();
    private bool _loading;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    private List<MatchReportDto> FilteredStandardGames { get; set; } = new List<MatchReportDto>();

    private List<MatchReportDto> FilteredFracturedGames { get; set; } = new List<MatchReportDto>();

    private List<MatchReportDto> FilteredGames => _selectedLeague == TiglLeague.Fractured
        ? FilteredFracturedGames
        : FilteredStandardGames;

    private string ActiveTabId => _selectedLeague == TiglLeague.Fractured
        ? "tigl-games-tab-fractured"
        : "tigl-games-tab-standard";

    private string SeasonFilterButtonLabel => (_areSeasonFiltersVisible ? "Hide season filter" : "Show season filter")
        + (IsSeasonFilterApplied ? ", filter active" : string.Empty);

    private string SeasonFilterIconPath => PathProvider.GetIconPath(_areSeasonFiltersVisible || IsSeasonFilterApplied ? IconType.FilterClicked : IconType.Filter);

    private bool IsSeasonFilterApplied => _selectedSeasonNumber != _defaultSeasonNumber;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;

        var seasonsTask = LoadSeasons();
        var gamesTask = LoadGameReports();
        await Task.WhenAll(seasonsTask, gamesTask);

        _defaultSeasonNumber = _seasons.Count > 0 ? _seasons.Max(s => s.SeasonNumber) : 0;
        _selectedSeasonNumber = _defaultSeasonNumber;
        UpdateSelectedGames();

        _loading = false;
    }

    private static string FormatEndDate(long endTs)
    {
        if (endTs <= 0)
            return "-";

        var dt = DateTimeOffset.FromUnixTimeMilliseconds(endTs).ToUniversalTime().DateTime;
        return dt.ToString("yyyy-MM-dd HH:mm", System.Globalization.CultureInfo.InvariantCulture);
    }

    private static string GetGameDetailUrl(int id)
    {
        return $"{Pages.Pages.TiglGameDetail}?id={id}";
    }

    private async Task LoadSeasons()
    {
        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            _seasons = resp.Data.Items;
        }
        else
        {
            _seasons = Array.Empty<SeasonDto>();
        }
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
        }
    }

    private void OnSeasonChanged(int seasonNumber)
    {
        _selectedSeasonNumber = seasonNumber;

        UpdateSelectedGames();
    }

    private string GetSelectedTitle() => $"{FilteredGames.Count} Games";

    private void ToggleSeasonFilters()
    {
        _areSeasonFiltersVisible = !_areSeasonFiltersVisible;
    }

    private void OnLeagueChanged(TiglLeague league)
    {
        _selectedLeague = league;
    }

    private void RedirectToGame(MatchReportDto game)
    {
        NavigationManager.NavigateTo(GetGameDetailUrl(game.Id));
    }

    private void UpdateSelectedGames()
    {
        FilteredStandardGames = _allStandard.Where(x => x.Season == _selectedSeasonNumber).ToList();
        FilteredFracturedGames = _allFractured.Where(x => x.Season == _selectedSeasonNumber).ToList();
    }
}
