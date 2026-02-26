using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Services.Tigl;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglStatisticsFactionsGrid
{
    private const int ThundersEdgeFirstSeason = 14;

    private bool _loading;
    private List<FactionSeasonStatsDto> _rows = new();

    private TiglFactionStatsShowBy _showBy = TiglFactionStatsShowBy.Era;
    private TiglFactionStatsEra _era = TiglFactionStatsEra.All;
    private TiglFactionStatsLeague _leagueFilter = TiglFactionStatsLeague.Standard;
    private int _season = -1;

    private bool ShouldShowLeagueFilter =>
        _showBy != TiglFactionStatsShowBy.Seasons
        || _season == -1
        || _season >= ThundersEdgeFirstSeason;

    [Inject] private ITwilightImperiumApiHttpClient HttpClient { get; set; } = default!;

    [Inject] private ITiglDataCache TiglCache { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task OnShowByChanged(TiglFactionStatsShowBy value)
    {
        _showBy = value;
        _leagueFilter = TiglFactionStatsLeague.Standard;
        await LoadAsync();
    }

    private async Task OnEraChanged(TiglFactionStatsEra value)
    {
        _era = value;
        await LoadAsync();
    }

    private async Task OnSeasonChanged(int seasonNumber)
    {
        _season = seasonNumber;

        if (_season != -1 && _season < ThundersEdgeFirstSeason)
        {
            _leagueFilter = TiglFactionStatsLeague.Standard;
        }

        await LoadAsync();
    }

    private async Task OnLeagueFilterChanged(TiglFactionStatsLeague value)
    {
        _leagueFilter = value;
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        StateHasChanged();

        var calls = BuildApiCalls();

        if (calls.Count == 0)
        {
            _rows = new List<FactionSeasonStatsDto>();
            _loading = false;
            return;
        }

        var results = new List<FactionSeasonStatsDto>();
        foreach (var (season, league) in calls)
        {
            var data = await FetchAsync(season, league);
            results.AddRange(data);
        }

        _rows = calls.Count > 1 ? MergeStats(results) : results;
        _loading = false;
    }

    private List<(int Season, TiglLeague League)> BuildApiCalls()
    {
        var calls = new List<(int Season, TiglLeague League)>();

        if (_showBy == TiglFactionStatsShowBy.Era)
        {
            switch (_era)
            {
                case TiglFactionStatsEra.All when _leagueFilter == TiglFactionStatsLeague.Standard:
                    calls.Add((-1, TiglLeague.ThundersEdge));
                    calls.Add((-1, TiglLeague.ProphecyOfKings));
                    break;
                case TiglFactionStatsEra.All:
                    calls.Add((-1, TiglLeague.Fractured));
                    break;
                case TiglFactionStatsEra.ThundersEdge:
                    calls.Add((-1, _leagueFilter == TiglFactionStatsLeague.Standard
                        ? TiglLeague.ThundersEdge
                        : TiglLeague.Fractured));
                    break;
                case TiglFactionStatsEra.ProphecyOfKings when _leagueFilter == TiglFactionStatsLeague.Standard:
                    calls.Add((-1, TiglLeague.ProphecyOfKings));
                    break;
                case TiglFactionStatsEra.ProphecyOfKings:
                    break;
            }
        }
        else
        {
            if (_season == -1)
            {
                if (_leagueFilter == TiglFactionStatsLeague.Standard)
                {
                    calls.Add((-1, TiglLeague.ThundersEdge));
                    calls.Add((-1, TiglLeague.ProphecyOfKings));
                }
                else
                {
                    calls.Add((-1, TiglLeague.Fractured));
                }
            }
            else
            {
                if (_season >= ThundersEdgeFirstSeason)
                {
                    calls.Add((_season, _leagueFilter == TiglFactionStatsLeague.Standard
                        ? TiglLeague.ThundersEdge
                        : TiglLeague.Fractured));
                }
                else
                {
                    calls.Add((_season, TiglLeague.ProphecyOfKings));
                }
            }
        }

        return calls;
    }

    private async Task<List<FactionSeasonStatsDto>> FetchAsync(int season, TiglLeague league)
    {
        var cached = TiglCache.GetFactionStats(season, league);
        if (cached is not null)
            return cached.ToList();

        var (resp, status) = await HttpClient.GetAsync<ApiResponse<ItemListDto<FactionSeasonStatsDto>>>(
            $"api/tigl/faction-stats/{season}/{league}");

        if (status == HttpStatusCode.OK && resp?.Data?.Items is not null)
        {
            var items = resp.Data.Items.ToList();
            TiglCache.UpdateFactionStats(season, league, items);
            return items;
        }

        return new List<FactionSeasonStatsDto>();
    }

    private static List<FactionSeasonStatsDto> MergeStats(List<FactionSeasonStatsDto> items)
    {
        return items
            .GroupBy(x => x.Faction)
            .Select(g =>
            {
                var totalGames = g.Sum(x => x.GamesPlayed);
                var totalWins = g.Sum(x => x.Wins);
                var totalScore = g.Sum(x => x.TotalScore);
                return new FactionSeasonStatsDto
                {
                    Faction = g.Key,
                    GamesPlayed = totalGames,
                    Wins = totalWins,
                    TotalScore = totalScore,
                    WinRate = totalGames > 0 ? (double)totalWins / totalGames * 100 : 0,
                    AverageScore = totalGames > 0 ? (double)totalScore / totalGames : 0,
                };
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