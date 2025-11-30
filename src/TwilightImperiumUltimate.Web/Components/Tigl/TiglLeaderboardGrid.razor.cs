using TwilightImperiumUltimate.Contracts.ApiContracts.Tigl.Season;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglLeaderboardGrid
{
    private bool _loading;
    private int _placement;
    private TiglLeague _selectedLeague = TiglLeague.ProphecyOfKings;
    private RankingSystem _selectedRankingSystem = RankingSystem.TrueSkill;
    private IReadOnlyCollection<SeasonDto> _seasons = Array.Empty<SeasonDto>();

    // Key: SeasonNumber => Value: League => List of results
    private readonly Dictionary<int, Dictionary<TiglLeague, List<PlayerSeasonResultDto>>> _seasonLeagueResults = [];

    // Currently selected season (last season by default once loaded)
    private int _selectedSeasonNumber;

    // Convenience cache of rows for currently selected season & league
    private List<PlayerSeasonResultDto> _currentRows = [];

    [Inject]
    private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadSeasonsAndLastSeasonResults();
    }

    private async Task LoadSeasonsAndLastSeasonResults()
    {
        _loading = true;
        StateHasChanged();
        await LoadSeasons();

        if (_selectedSeasonNumber > 0)
        {
            await LoadSeasonResults(_selectedSeasonNumber);
            UpdateCurrentRows();
        }

        _loading = false;
        StateHasChanged();
    }

    private async Task LoadSeasons()
    {
        if (_seasons.Count > 0)
            return;

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

        var (lbResponse, lbStatus) = await HttpClient.GetAsync<ApiResponse<ItemListDto<PlayerSeasonResultDto>>>(string.Concat(Paths.ApiPath_SeasonLeaderboard, seasonNumber));
        if (lbStatus == HttpStatusCode.OK && lbResponse?.Data?.Items is not null && lbResponse.Data.Items.Count != 0)
        {
            var grouped = lbResponse.Data.Items
                .GroupBy(r => r.League)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(r =>
                {
                    if (_selectedRankingSystem == RankingSystem.Async)
                        return r.AsyncRating;
                    if (_selectedRankingSystem == RankingSystem.Glicko2)
                        return r.GlickoRating;
                    return r.TrueSkillConservativeRating;
                }).ToList());

            if (grouped.Count > 0)
            {
                _seasonLeagueResults[seasonNumber] = grouped;
            }
        }
    }

    private void OnLeagueChanged(TiglLeague league)
    {
        _selectedLeague = league;
        UpdateCurrentRows();
    }

    private void OnRankingSystemChanged(RankingSystem rankingSystem)
    {
        _selectedRankingSystem = rankingSystem;
        StateHasChanged();
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
        return _currentRows
            .OrderByDescending(r =>
            {
                if (_selectedRankingSystem == RankingSystem.Async)
                    return r.AsyncRating;
                if (_selectedRankingSystem == RankingSystem.Glicko2)
                    return r.GlickoRating;
                return r.TrueSkillConservativeRating;
            })
            .ToList();
    }

    public static TextColor GetWinrateColor(double winrate)
    {
        if (winrate > 16.67f) return TextColor.Green;
        if (winrate > 12.0f) return TextColor.Yellow;
        if (winrate > 8.0f) return TextColor.Orange;
        return TextColor.Red;
    }
}