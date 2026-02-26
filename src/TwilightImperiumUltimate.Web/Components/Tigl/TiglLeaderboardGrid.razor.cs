using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Services.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglLeaderboardGrid
{
    private const int PageSize = 50;
    private int _currentPage = 1;

    private bool _loading;
    private int _placement;
    private TiglLeague _selectedLeague = TiglLeague.ProphecyOfKings;
    private TiglLeagueFilter _selectedLeagueFilter = TiglLeagueFilter.Standard;
    private RankingSystem _selectedRankingSystem = RankingSystem.TrueSkill;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    private bool _onlyActive = true;
    private bool _onlyConfident = true;

    // Key: SeasonNumber => Value: League => List of results
    private readonly Dictionary<int, Dictionary<TiglLeague, List<PlayerSeasonResultDto>>> _seasonLeagueResults = [];

    // Currently selected season (last season by default once loaded)
    private int _selectedSeasonNumber;

    // Convenience cache of rows for currently selected season & league
    private List<PlayerSeasonResultDto> _currentRows = [];
    private int _filteredCount = -1;

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private ITiglDataCache TiglCache { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public static TextColor GetWinrateColor(double winrate)
    {
        if (winrate > 16.67f) return TextColor.Green;
        if (winrate > 12.0f) return TextColor.Yellow;
        if (winrate > 8.0f) return TextColor.Orange;
        return TextColor.Red;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadSeasonsAndLastSeasonResults();
    }

    private async Task LoadSeasonsAndLastSeasonResults()
    {
        _loading = true;

        await LoadSeasons();
        StateHasChanged();

        if (_selectedSeasonNumber > 0)
        {
            await LoadSeasonResults(_selectedSeasonNumber);
            UpdateCurrentRows();
        }

        _loading = false;
    }

    private async Task LoadSeasons()
    {
        var (seasonsResponse, seasonsStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<SeasonDto>>>(Paths.ApiPath_Seasons);
        if (seasonsStatus == HttpStatusCode.OK && seasonsResponse?.Data?.Items is not null)
        {
            _seasons = seasonsResponse.Data.Items;
        }
        else
        {
            _seasons = Array.Empty<SeasonDto>();
        }

        _selectedSeasonNumber = _seasons.Count > 0 ? _seasons.Max(s => s.SeasonNumber) : 0;
    }

    // Loads leaderboard for a season and stores grouped by league if not already loaded
    private async Task LoadSeasonResults(int seasonNumber)
    {
        if (seasonNumber <= 0)
            return;

        if (_seasonLeagueResults.ContainsKey(seasonNumber))
            return;

        var cached = TiglCache.GetLeaderboard(seasonNumber);
        if (cached is not null)
        {
            GroupAndStore(seasonNumber, cached);
            return;
        }

        var (lbResponse, lbStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<PlayerSeasonResultDto>>>(string.Concat(Paths.ApiPath_SeasonLeaderboard, seasonNumber));
        if (lbStatus == HttpStatusCode.OK && lbResponse?.Data?.Items is not null && lbResponse.Data.Items.Count != 0)
        {
            TiglCache.UpdateLeaderboard(seasonNumber, lbResponse.Data.Items.ToList());
            GroupAndStore(seasonNumber, lbResponse.Data.Items);
        }
    }

    private void GroupAndStore(int seasonNumber, IReadOnlyCollection<PlayerSeasonResultDto> items)
    {
        var grouped = items
            .GroupBy(r => r.League)
            .ToDictionary(g => g.Key, g => g.ToList());

        if (grouped.Count > 0)
        {
            _seasonLeagueResults[seasonNumber] = grouped;
        }
    }

    private void OnLeagueChanged(TiglLeagueFilter leagueFilter)
    {
        if (leagueFilter == TiglLeagueFilter.Standard)
            _selectedLeague = TiglLeague.ProphecyOfKings;
        else
            _selectedLeague = TiglLeague.Fractured;

        _selectedLeagueFilter = leagueFilter;

        UpdateCurrentRows();
    }

    private void OnRankingSystemChanged(RankingSystem rankingSystem)
    {
        _selectedRankingSystem = rankingSystem;
        _currentPage = 1;
        _filteredCount = -1;
    }

    private void UpdateCurrentRows()
    {
        if (_seasonLeagueResults.TryGetValue(_selectedSeasonNumber, out var leagueDict) && leagueDict.TryGetValue(_selectedLeague, out var rows))
        {
            _currentRows = rows;
        }
        else
        {
            _currentRows = [];
        }

        _currentPage = 1;
        _filteredCount = -1;
    }

    private async Task DecreaseSeasonNumber()
    {
        _loading = true;
        StateHasChanged();

        var maxSeason = _seasons.Max(x => x.SeasonNumber);
        var minSeason = _seasons.Min(x => x.SeasonNumber);

        if (_selectedSeasonNumber > minSeason)
            _selectedSeasonNumber--;
        else if (_selectedSeasonNumber == minSeason)
            _selectedSeasonNumber = maxSeason;

        await LoadSeasonResults(_selectedSeasonNumber);
        UpdateCurrentRows();

        _loading = false;
        StateHasChanged();
    }

    private async Task IncreaseSeasonNumber()
    {
        _loading = true;
        StateHasChanged();

        var maxSeason = _seasons.Max(x => x.SeasonNumber);
        var minSeason = _seasons.Min(x => x.SeasonNumber);

        if (_selectedSeasonNumber < maxSeason)
            _selectedSeasonNumber++;
        else if (_selectedSeasonNumber == maxSeason)
            _selectedSeasonNumber = minSeason;

        await LoadSeasonResults(_selectedSeasonNumber);
        UpdateCurrentRows();

        _loading = false;
        StateHasChanged();
    }

    private List<PlayerSeasonResultDto> GetSortedRows()
    {
        return ApplyFilters(_currentRows)
            .OrderByDescending(r =>
            {
                if (_selectedRankingSystem == RankingSystem.Async)
                    return r.AsyncRating;
                if (_selectedRankingSystem == RankingSystem.Glicko2)
                    return r.GlickoRating;
                return r.TrueSkillConservativeRating;
            })
            .Skip((_currentPage - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }

    private IEnumerable<PlayerSeasonResultDto> ApplyFilters(IEnumerable<PlayerSeasonResultDto> source)
    {
        var query = source;

        if (_onlyActive)
        {
            query = query.Where(r => r.IsActive);
        }

        if (_onlyConfident)
        {
            if (_selectedRankingSystem == RankingSystem.Glicko2)
            {
                query = query.Where(r => r.GlickoRd < 42.0);
            }
            else if (_selectedRankingSystem == RankingSystem.TrueSkill)
            {
                query = query.Where(r => r.TrueSkillSigma < 0.8);
            }
            else if (_selectedRankingSystem == RankingSystem.Async)
            {
                query = query.Where(r => r.GamesPlayed >= 40);
            }
        }

        return query;
    }

    private void OnPageChanged(int newPage)
    {
        _currentPage = newPage;
    }

    private int GetTotalFilteredCount()
    {
        if (_filteredCount >= 0)
            return _filteredCount;

        _filteredCount = ApplyFilters(_currentRows).Count();
        return _filteredCount;
    }

    private void OnOnlyActiveChanged(bool value)
    {
        _onlyActive = value;
        _currentPage = 1;
        _filteredCount = -1;
    }

    private void OnOnlyConfidentChanged(bool value)
    {
        _onlyConfident = value;
        _currentPage = 1;
        _filteredCount = -1;
    }

    private void RedirectToPlayer(int playerId)
    {
        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglLeaderboard);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={playerId}&returnUrl={returnUrl}");
    }
}