using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Services.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglLeaderboardGrid
{
    private readonly Dictionary<int, Dictionary<TiglLeague, List<PlayerSeasonResultDto>>> _seasonLeagueResults = [];

    private bool _loading;
    private TiglLeague _selectedLeague = TiglLeague.ProphecyOfKings;
    private TiglLeagueFilter _selectedLeagueFilter = TiglLeagueFilter.Standard;
    private RankingSystem _selectedRankingSystem = RankingSystem.TrueSkill;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();
    private bool _onlyActive = true;
    private bool _onlyConfident = true;
    private int _selectedSeasonNumber;
    private List<PlayerSeasonResultDto> _currentRows = [];

    private IReadOnlyCollection<KeyValuePair<TiglLeagueFilter, string>> LeagueOptions =>
        EnumExtensions.GetEnumValuesWithDisplayNames<TiglLeagueFilter>();

    private IReadOnlyCollection<KeyValuePair<RankingSystem, string>> RankingSystemOptions =>
        EnumExtensions.GetEnumValuesWithDisplayNames<RankingSystem>();

    private IReadOnlyList<PlayerSeasonResultDto> DisplayedRows => GetSortedRows();

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject]
    private ITiglDataCache TiglCache { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    public static TextColor GetWinrateColor(double winrate)
    {
        if (winrate > 16.67)
            return TextColor.Green;
        if (winrate > 12.0)
            return TextColor.Yellow;
        if (winrate > 8.0)
            return TextColor.Orange;
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

    private async Task LoadSeasonResults(int seasonNumber)
    {
        if (seasonNumber <= 0 || _seasonLeagueResults.ContainsKey(seasonNumber))
            return;

        var cached = TiglCache.GetLeaderboard(seasonNumber);
        if (cached is not null)
        {
            GroupAndStore(seasonNumber, cached);
            return;
        }

        var (leaderboardResponse, leaderboardStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<PlayerSeasonResultDto>>>(string.Concat(Paths.ApiPath_SeasonLeaderboard, seasonNumber));
        if (leaderboardStatus == HttpStatusCode.OK && leaderboardResponse?.Data?.Items is not null && leaderboardResponse.Data.Items.Count != 0)
        {
            TiglCache.UpdateLeaderboard(seasonNumber, leaderboardResponse.Data.Items.ToList());
            GroupAndStore(seasonNumber, leaderboardResponse.Data.Items);
        }
    }

    private void GroupAndStore(int seasonNumber, IReadOnlyCollection<PlayerSeasonResultDto> items)
    {
        var grouped = items
            .GroupBy(result => result.League)
            .ToDictionary(group => group.Key, group => group.ToList());

        if (grouped.Count > 0)
        {
            _seasonLeagueResults[seasonNumber] = grouped;
        }
    }

    private async Task OnSeasonChanged(int seasonNumber)
    {
        _selectedSeasonNumber = seasonNumber;
        _loading = true;

        await LoadSeasonResults(seasonNumber);
        UpdateCurrentRows();

        _loading = false;
    }

    private void OnLeagueChanged(TiglLeagueFilter leagueFilter)
    {
        _selectedLeague = leagueFilter == TiglLeagueFilter.Standard
            ? TiglLeague.ProphecyOfKings
            : TiglLeague.Fractured;
        _selectedLeagueFilter = leagueFilter;
        UpdateCurrentRows();
    }

    private void OnRankingSystemChanged(RankingSystem rankingSystem)
    {
        _selectedRankingSystem = rankingSystem;
    }

    private void UpdateCurrentRows()
    {
        if (_seasonLeagueResults.TryGetValue(_selectedSeasonNumber, out var leagueResults) && leagueResults.TryGetValue(_selectedLeague, out var rows))
        {
            _currentRows = rows;
        }
        else
        {
            _currentRows = [];
        }
    }

    private List<PlayerSeasonResultDto> GetSortedRows()
    {
        return ApplyFilters(_currentRows)
            .OrderByDescending(GetRating)
            .ToList();
    }

    private double GetRating(PlayerSeasonResultDto playerResult) => _selectedRankingSystem switch
    {
        RankingSystem.Async => playerResult.AsyncRating,
        RankingSystem.Glicko2 => playerResult.GlickoRating,
        _ => playerResult.TrueSkillConservativeRating,
    };

    private IEnumerable<PlayerSeasonResultDto> ApplyFilters(IEnumerable<PlayerSeasonResultDto> source)
    {
        var query = source;

        if (_onlyActive)
        {
            query = query.Where(result => result.IsActive);
        }

        if (_onlyConfident)
        {
            query = _selectedRankingSystem switch
            {
                RankingSystem.Glicko2 => query.Where(result => result.GlickoRd < 42.0),
                RankingSystem.TrueSkill => query.Where(result => result.TrueSkillSigma < 0.8),
                RankingSystem.Async => query.Where(result => result.GamesPlayed >= 40),
                _ => query,
            };
        }

        return query;
    }

    private int GetPlacement(PlayerSeasonResultDto playerResult) => DisplayedRows.ToList().IndexOf(playerResult) + 1;

    private void OnOnlyActiveChanged(bool value)
    {
        _onlyActive = value;
    }

    private void OnOnlyConfidentChanged(bool value)
    {
        _onlyConfident = value;
    }

    private void RedirectToPlayer(PlayerSeasonResultDto playerResult)
    {
        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglLeaderboard);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={playerResult.TiglUserId}&returnUrl={returnUrl}");
    }
}
