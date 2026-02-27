using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;
using TwilightImperiumUltimate.Web.Helpers.Tigl;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileGrid
{
    private static readonly TiglLeague[] LeagueDisplayOrder = { TiglLeague.ThundersEdge, TiglLeague.Fractured, TiglLeague.ProphecyOfKings };

    private int _factionRow;
    private bool _showAllAchievements;
    private FactionStatisticsFilter _selectedFactionFilter = FactionStatisticsFilter.Official;
    private FactionStatisticsVpFilter _selectedScoreFilter = FactionStatisticsVpFilter.All;
    private readonly HashSet<string> _expandedSeasons = new();

    public TiglProfileCategory CurrentCategory { get; set; } = TiglProfileCategory.All;

    [CascadingParameter(Name = "TiglPlayerProfile")]
    public TiglPlayerProfileDto Profile { get; set; } = default!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private IReadOnlyList<TiglLeagueProfileDto> FilteredLeagueProfiles => GetFilteredLeagueProfiles();

    private IReadOnlyList<RankHistoryDto> FilteredRankHistory => GetFilteredRankHistory();

    private IReadOnlyList<PrestigeRankHistoryDto> FilteredPrestigeRanks => GetFilteredPrestigeRanks();

    private IReadOnlyList<TiglProfileGameDto> FilteredGameHistory => GetFilteredGameHistory();

    private IReadOnlyList<TiglTopOpponentDto> FilteredTopOpponents => GetFilteredTopOpponents();

    private IReadOnlyList<TiglProfileFactionStatsDto> FilteredFactionStats => GetFilteredFactionStats();

    private bool IsFactionFilterVisible => CurrentCategory == TiglProfileCategory.All || CurrentCategory == TiglProfileCategory.Fractured;

    private IEnumerable<TiglGamesByYear> GroupedFilteredGames => GetGroupedFilteredGames();

    private IReadOnlyList<SeasonSummary> FilteredSeasons => GetFilteredSeasons();

    private IReadOnlyList<AchievementDisplayEntry> DisplayedAchievements => GetDisplayedAchievements();

    private List<ChartSeries> AsyncChartSeries => BuildAsyncChartSeries();

    private List<ChartSeries> GlickoChartSeries => BuildGlickoChartSeries();

    private List<ChartSeries> TrueSkillChartSeries => BuildTrueSkillChartSeries();

    private void OnCategoryChanged(TiglProfileCategory category)
    {
        CurrentCategory = category;
        _selectedFactionFilter = FactionStatisticsFilter.Official;
        _selectedScoreFilter = FactionStatisticsVpFilter.All;
        _expandedSeasons.Clear();
        StateHasChanged();
    }

    private void OnFactionFilterChanged(FactionStatisticsFilter filter)
    {
        _selectedFactionFilter = filter;
        StateHasChanged();
    }

    private void OnScoreFilterChanged(FactionStatisticsVpFilter filter)
    {
        _selectedScoreFilter = filter;
        StateHasChanged();
    }

    private IReadOnlyList<TiglLeagueProfileDto> GetFilteredLeagueProfiles()
    {
        if (Profile?.LeagueProfiles is null)
            return Array.Empty<TiglLeagueProfileDto>();

        if (CurrentCategory == TiglProfileCategory.All)
            return Profile.LeagueProfiles
                .OrderBy(l => Array.IndexOf(LeagueDisplayOrder, l.League))
                .ToList();

        var league = MapCategoryToLeague(CurrentCategory);
        return Profile.LeagueProfiles.Where(l => l.League == league).ToList();
    }

    private IReadOnlyList<RankHistoryDto> GetFilteredRankHistory()
    {
        if (Profile?.RankHistory is null)
            return Array.Empty<RankHistoryDto>();

        if (CurrentCategory == TiglProfileCategory.All)
            return Profile.RankHistory;

        var league = MapCategoryToLeague(CurrentCategory);
        return Profile.RankHistory.Where(r => r.League == league).ToList();
    }

    private IReadOnlyList<PrestigeRankHistoryDto> GetFilteredPrestigeRanks()
    {
        if (Profile?.PrestigeRankHistory is null)
            return Array.Empty<PrestigeRankHistoryDto>();

        var source = CurrentCategory == TiglProfileCategory.All
            ? Profile.PrestigeRankHistory.AsEnumerable()
            : Profile.PrestigeRankHistory.Where(p => p.League == MapCategoryToLeague(CurrentCategory));

        return source
            .OrderByDescending(p => p.AchievedAt)
            .ThenBy(p => p.Faction != TiglFactionName.None ? 1 : 0)
            .ToList();
    }

    private IReadOnlyList<TiglProfileGameDto> GetFilteredGameHistory()
    {
        if (Profile?.GameHistory is null)
            return Array.Empty<TiglProfileGameDto>();

        if (CurrentCategory == TiglProfileCategory.All)
            return Profile.GameHistory;

        var league = MapCategoryToLeague(CurrentCategory);
        return Profile.GameHistory.Where(g => g.League == league).ToList();
    }

    private IReadOnlyList<TiglTopOpponentDto> GetFilteredTopOpponents()
    {
        if (Profile?.TopOpponents is null)
            return Array.Empty<TiglTopOpponentDto>();

        if (CurrentCategory == TiglProfileCategory.All)
        {
            return Profile.TopOpponents
                .GroupBy(o => new { o.TiglUserId, o.TiglUserName })
                .Select(g => new TiglTopOpponentDto
                {
                    TiglUserId = g.Key.TiglUserId,
                    TiglUserName = g.Key.TiglUserName,
                    GamesPlayed = g.Sum(x => x.GamesPlayed),
                })
                .OrderByDescending(o => o.GamesPlayed)
                .Take(20)
                .ToList();
        }

        var league = MapCategoryToLeague(CurrentCategory);
        return Profile.TopOpponents
            .Where(o => o.League == league)
            .OrderByDescending(o => o.GamesPlayed)
            .Take(20)
            .ToList();
    }

    private IReadOnlyList<TiglProfileFactionStatsDto> GetFilteredFactionStats()
    {
        if (Profile?.FactionStats is null)
            return Array.Empty<TiglProfileFactionStatsDto>();

        var effectiveFilter = IsFactionFilterVisible
            ? _selectedFactionFilter
            : FactionStatisticsFilter.Official;

        var factionNames = GetFactionsByFilter(effectiveFilter);

        var scoreFilteredGames = GetScoreFilteredGameHistory();

        Dictionary<TiglFactionName, TiglProfileFactionStatsDto> statsLookup;

        if (CurrentCategory == TiglProfileCategory.All)
        {
            statsLookup = scoreFilteredGames
                .GroupBy(g => g.Faction)
                .ToDictionary(
                    g => g.Key,
                    g => new TiglProfileFactionStatsDto
                    {
                        Faction = g.Key,
                        Games = g.Count(),
                        Wins = g.Count(x => x.IsWinner),
                        MinVp = g.Any() ? g.Min(x => x.Score) : 0,
                        MaxVp = g.Any() ? g.Max(x => x.Score) : 0,
                        TotalScoredVp = g.Sum(x => x.Score),
                        MaxPossibleVp = g.Sum(x => x.MaxScore),
                    });
        }
        else
        {
            var league = MapCategoryToLeague(CurrentCategory);
            statsLookup = scoreFilteredGames
                .Where(g => g.League == league)
                .GroupBy(g => g.Faction)
                .ToDictionary(
                    g => g.Key,
                    g => new TiglProfileFactionStatsDto
                    {
                        Faction = g.Key,
                        League = league,
                        Games = g.Count(),
                        Wins = g.Count(x => x.IsWinner),
                        MinVp = g.Any() ? g.Min(x => x.Score) : 0,
                        MaxVp = g.Any() ? g.Max(x => x.Score) : 0,
                        TotalScoredVp = g.Sum(x => x.Score),
                        MaxPossibleVp = g.Sum(x => x.MaxScore),
                    });
        }

        return factionNames
            .Select(name => statsLookup.TryGetValue(name, out var stats)
                ? stats
                : new TiglProfileFactionStatsDto { Faction = name })
            .ToList();
    }

    private IReadOnlyList<TiglProfileGameDto> GetScoreFilteredGameHistory()
    {
        if (Profile?.GameHistory is null)
            return Array.Empty<TiglProfileGameDto>();

        var games = Profile.GameHistory.AsEnumerable();

        if (_selectedScoreFilter != FactionStatisticsVpFilter.All)
        {
            var targetScore = _selectedScoreFilter switch
            {
                FactionStatisticsVpFilter.TenVp => 10,
                FactionStatisticsVpFilter.TwelveVp => 12,
                FactionStatisticsVpFilter.FourteenVp => 14,
                _ => 0,
            };

            if (targetScore > 0)
                games = games.Where(g => g.MaxScore == targetScore);
        }

        return games.ToList();
    }

    private static IReadOnlyList<TiglFactionName> GetFactionsByFilter(FactionStatisticsFilter filter)
    {
        return filter switch
        {
            FactionStatisticsFilter.Official => Enum.GetValues<TiglFactionName>()
                .Where(f => f >= TiglFactionName.TheArborec && f <= TiglFactionName.TheRalNelConsortium)
                .ToList(),
            FactionStatisticsFilter.DiscordantStars => Enum.GetValues<TiglFactionName>()
                .Where(f => f >= TiglFactionName.TheAugursOfIlyxum && f <= TiglFactionName.TheZelianPurifier)
                .ToList(),
            FactionStatisticsFilter.Others => Enum.GetValues<TiglFactionName>()
                .Where(f => (f >= TiglFactionName.TheRubyMonarch && f <= TiglFactionName.ASickeningLurch) ||
                            (f >= TiglFactionName.UydaiConclave && f <= TiglFactionName.TwilightsFall))
                .ToList(),
            _ => Array.Empty<TiglFactionName>(),
        };
    }

    private IEnumerable<TiglGamesByYear> GetGroupedFilteredGames()
    {
        return FilteredGameHistory
            .GroupBy(game => DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp).Year)
            .OrderByDescending(yearGroup => yearGroup.Key)
            .Select(yearGroup => new TiglGamesByYear
            {
                Year = new DateOnly(yearGroup.Key, 1, 1),
                Months = yearGroup
                    .GroupBy(game => DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp).Month)
                    .OrderByDescending(monthGroup => monthGroup.Key)
                    .Select(monthGroup => new TiglGamesByMonth
                    {
                        Month = new DateOnly(yearGroup.Key, monthGroup.Key, 1),
                        Games = monthGroup.OrderByDescending(x => x.EndTimestamp),
                    }),
            });
    }

    private IReadOnlyList<SeasonSummary> GetFilteredSeasons()
    {
        if (Profile?.GameHistory is null)
            return Array.Empty<SeasonSummary>();

        var games = CurrentCategory == TiglProfileCategory.All
            ? Profile.GameHistory.AsEnumerable()
            : Profile.GameHistory.Where(g => g.League == MapCategoryToLeague(CurrentCategory));

        return games
            .GroupBy(g => new { g.League, g.Season })
            .OrderBy(g => Array.IndexOf(LeagueDisplayOrder, g.Key.League))
            .ThenByDescending(g => g.Key.Season)
            .Select(g => new SeasonSummary
            {
                League = g.Key.League,
                Season = g.Key.Season,
                GamesPlayed = g.Count(),
                Wins = g.Count(x => x.IsWinner),
                Games = g.OrderByDescending(x => x.EndTimestamp).ToList(),
            })
            .ToList();
    }

    private void ToggleSeason(SeasonSummary season)
    {
        var key = $"{season.League}_{season.Season}";
        if (!_expandedSeasons.Remove(key))
            _expandedSeasons.Add(key);
        StateHasChanged();
    }

    private bool IsSeasonExpanded(SeasonSummary season)
        => _expandedSeasons.Contains($"{season.League}_{season.Season}");

    private double GetLeagueWinRate(TiglLeagueProfileDto league)
    {
        var games = Profile?.GameHistory?.Where(g => g.League == league.League).ToList();
        if (games is null || games.Count == 0)
            return 0;
        return (double)games.Count(g => g.IsWinner) / games.Count * 100;
    }

    private double GetHighestAsyncRating(TiglLeagueProfileDto league)
    {
        if (league.AsyncMatchHistory.Count == 0)
            return league.AsyncRating;
        return Math.Max(league.AsyncRating, league.AsyncMatchHistory.Max(m => m.RatingNew));
    }

    private double GetHighestGlickoRating(TiglLeagueProfileDto league)
    {
        if (league.GlickoMatchHistory.Count == 0)
            return league.GlickoRating;
        return Math.Max(league.GlickoRating, league.GlickoMatchHistory.Max(m => m.RatingNew));
    }

    private double GetHighestTrueSkillRating(TiglLeagueProfileDto league)
    {
        if (league.TrueSkillMatchHistory.Count == 0)
            return league.TrueSkillMu;
        return Math.Max(league.TrueSkillMu, league.TrueSkillMatchHistory.Max(m => m.MuNew));
    }

    private double GetHighestTrueSkillConservative(TiglLeagueProfileDto league)
    {
        if (league.TrueSkillMatchHistory.Count == 0)
            return league.TrueSkillConservative;
        return Math.Max(league.TrueSkillConservative, league.TrueSkillMatchHistory.Max(m => m.ConservativeRatingNew));
    }

    private static TextColor GetPrestigeColor(TiglPrestigeRank prestigeRank)
    {
        return prestigeRank switch
        {
            TiglPrestigeRank.PaxMagnificaBellumGloriosum => TextColor.Pmbg,
            TiglPrestigeRank.GalacticThreat => TextColor.GalacticThreat,
            TiglPrestigeRank.Tyrant => TextColor.Tyrant,
            _ => TextColor.White,
        };
    }

    private PrestigeRankHistoryDto? GetLeaguePrestigeTitle(TiglLeague league)
    {
        return Profile?.PrestigeRankHistory?
            .Where(p => p.League == league &&
                        p.PrestigeRank is TiglPrestigeRank.GalacticThreat
                            or TiglPrestigeRank.Tyrant
                            or TiglPrestigeRank.PaxMagnificaBellumGloriosum)
            .OrderByDescending(p => p.Level)
            .ThenByDescending(p => p.AchievedAt)
            .FirstOrDefault();
    }

    private void NavigateToGameDetail(int matchReportId)
    {
        var returnUrl = $"{Web.Pages.Pages.TiglPlayerProfile}?playerId={Profile.TiglUserId}";
        NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglGameDetail}?id={matchReportId}&returnUrl={Uri.EscapeDataString(returnUrl)}");
    }

    private void NavigateToPlayerProfile(int tiglUserId)
    {
        var currentUrl = NavigationManager.Uri;
        var returnUrl = Uri.EscapeDataString(currentUrl);
        NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}");
    }

    private List<ChartSeries> BuildAsyncChartSeries()
    {
        var series = new List<ChartSeries>();
        if (Profile?.LeagueProfiles is null)
            return series;

        var profiles = CurrentCategory == TiglProfileCategory.All
            ? Profile.LeagueProfiles.OrderBy(l => Array.IndexOf(LeagueDisplayOrder, l.League)).ToList()
            : Profile.LeagueProfiles.Where(l => l.League == MapCategoryToLeague(CurrentCategory)).ToList();

        foreach (var league in profiles)
        {
            if (league.AsyncMatchHistory.Count == 0)
                continue;

            var points = league.AsyncMatchHistory
                .Select((m, i) => new ChartDataPoint { Game = i + 1, Rating = m.RatingNew })
                .ToList();

            series.Add(new ChartSeries
            {
                Title = CurrentCategory == TiglProfileCategory.All ? league.League.GetDisplayName() : "Async Rating",
                Points = points,
            });
        }

        return series;
    }

    private List<ChartSeries> BuildGlickoChartSeries()
    {
        var series = new List<ChartSeries>();
        if (Profile?.LeagueProfiles is null)
            return series;

        var profiles = CurrentCategory == TiglProfileCategory.All
            ? Profile.LeagueProfiles.OrderBy(l => Array.IndexOf(LeagueDisplayOrder, l.League)).ToList()
            : Profile.LeagueProfiles.Where(l => l.League == MapCategoryToLeague(CurrentCategory)).ToList();

        foreach (var league in profiles)
        {
            if (league.GlickoMatchHistory.Count == 0)
                continue;

            var points = league.GlickoMatchHistory
                .Select((m, i) => new ChartDataPoint { Game = i + 1, Rating = m.RatingNew })
                .ToList();

            series.Add(new ChartSeries
            {
                Title = CurrentCategory == TiglProfileCategory.All ? league.League.GetDisplayName() : "Glicko-2 Rating",
                Points = points,
            });
        }

        return series;
    }

    private List<ChartSeries> BuildTrueSkillChartSeries()
    {
        var series = new List<ChartSeries>();
        if (Profile?.LeagueProfiles is null)
            return series;

        var profiles = CurrentCategory == TiglProfileCategory.All
            ? Profile.LeagueProfiles.OrderBy(l => Array.IndexOf(LeagueDisplayOrder, l.League)).ToList()
            : Profile.LeagueProfiles.Where(l => l.League == MapCategoryToLeague(CurrentCategory)).ToList();

        foreach (var league in profiles)
        {
            if (league.TrueSkillMatchHistory.Count == 0)
                continue;

            var muPoints = league.TrueSkillMatchHistory
                .Select((m, i) => new ChartDataPoint { Game = i + 1, Rating = m.MuNew })
                .ToList();

            var conservativePoints = league.TrueSkillMatchHistory
                .Select((m, i) => new ChartDataPoint { Game = i + 1, Rating = m.ConservativeRatingNew })
                .ToList();

            var prefix = CurrentCategory == TiglProfileCategory.All ? $"{league.League.GetDisplayName()} " : string.Empty;

            series.Add(new ChartSeries { Title = $"{prefix}Conservative", Points = conservativePoints });
            series.Add(new ChartSeries { Title = $"{prefix}Rating", Points = muPoints });
        }

        return series;
    }

    private double GetRarityPercent(AchievementName achievementName)
    {
        if (Profile is null || Profile.TotalTiglUsers == 0)
            return 0;

        var key = achievementName.ToString();
        if (Profile.AchievementPlayerCounts.TryGetValue(key, out var count))
        {
            return (double)count / Profile.TotalTiglUsers * 100.0;
        }

        return 0;
    }

    private IReadOnlyList<AchievementDisplayEntry> GetDisplayedAchievements()
    {
        if (Profile is null)
            return Array.Empty<AchievementDisplayEntry>();

        var earnedSet = Profile.Achievements
            .ToDictionary(a => a.AchievementName);

        var earned = Profile.Achievements
            .OrderBy(a => GetRarityPercent(a.AchievementName))
            .Select(a => new AchievementDisplayEntry
            {
                Achievement = a,
                RarityPercent = GetRarityPercent(a.AchievementName),
                IsEarned = true,
            })
            .ToList();

        if (!_showAllAchievements)
            return earned;

        var unearned = Enum.GetValues<AchievementName>()
            .Where(name => !earnedSet.ContainsKey(name))
            .Select(name => new AchievementDisplayEntry
            {
                Achievement = new TiglUserAchievementDto { AchievementName = name },
                RarityPercent = GetRarityPercent(name),
                IsEarned = false,
            })
            .OrderBy(e => e.RarityPercent)
            .ToList();

        earned.AddRange(unearned);
        return earned;
    }

    private void OnShowAllAchievementsChanged(ChangeEventArgs e)
    {
        _showAllAchievements = (bool)(e.Value ?? false);
        StateHasChanged();
    }

    private static string FormatTimestamp(long unixTimestamp)
    {
        if (unixTimestamp <= 0)
            return "N/A";

        var dt = DateTimeOffset.FromUnixTimeMilliseconds(unixTimestamp).UtcDateTime;
        return dt.ToString("yyyy-MM-dd");
    }

    private static TiglLeague MapCategoryToLeague(TiglProfileCategory category) => category switch
    {
        TiglProfileCategory.ProphecyOfKings => TiglLeague.ProphecyOfKings,
        TiglProfileCategory.ThundersEdge => TiglLeague.ThundersEdge,
        TiglProfileCategory.Fractured => TiglLeague.Fractured,
        _ => TiglLeague.ProphecyOfKings,
    };

    public class ChartDataPoint
    {
        public int Game { get; set; }

        public double Rating { get; set; }
    }

    public class ChartSeries
    {
        public string Title { get; set; } = string.Empty;

        public List<ChartDataPoint> Points { get; set; } = new();
    }

    private static (double Min, double Max, double Step) GetChartAxisBounds(List<ChartSeries> seriesList)
    {
        var allRatings = seriesList.SelectMany(s => s.Points).Select(p => p.Rating).ToList();
        if (allRatings.Count == 0)
            return (0, 10, 2);

        var min = allRatings.Min();
        var max = allRatings.Max();

        var axisMin = Math.Floor(min) - 1;
        var axisMax = Math.Ceiling(max) + 1;

        var range = axisMax - axisMin;
        if (range < 5)
        {
            var mid = (axisMin + axisMax) / 2.0;
            axisMin = mid - 2.5;
            axisMax = mid + 2.5;
            range = 5;
        }

        var step = range / 5.0;

        var niceSteps = new[] { 0.1, 0.2, 0.25, 0.5, 1.0, 2.0, 5.0, 10.0, 20.0, 25.0, 50.0, 100.0, 200.0, 250.0, 500.0 };
        var niceStep = niceSteps.FirstOrDefault(s => s >= step);
        if (niceStep == 0)
            niceStep = step;

        axisMin = Math.Floor(axisMin / niceStep) * niceStep;
        axisMax = Math.Ceiling(axisMax / niceStep) * niceStep;

        return (axisMin, axisMax, niceStep);
    }

    private static string GetDurationTime(TiglProfileGameDto game)
    {
        if (game.StartTimestamp <= 0 || game.EndTimestamp <= 0)
            return "N/A";

        var start = DateTimeOffset.FromUnixTimeMilliseconds(game.StartTimestamp);
        var end = DateTimeOffset.FromUnixTimeMilliseconds(game.EndTimestamp);
        var duration = end - start;

        var parts = new List<string>();
        if (duration.Days > 0)
            parts.Add($"{duration.Days:D2} d");
        if (duration.Hours > 0 || duration.Days > 0)
            parts.Add($"{duration.Hours:D2} h");

        return parts.Count > 0 ? string.Join(" ", parts) : string.Empty;
    }

    public class TiglGamesByYear
    {
        public DateOnly Year { get; set; }

        public IEnumerable<TiglGamesByMonth> Months { get; set; } = new List<TiglGamesByMonth>();
    }

    public class TiglGamesByMonth
    {
        public DateOnly Month { get; set; }

        public IEnumerable<TiglProfileGameDto> Games { get; set; } = new List<TiglProfileGameDto>();
    }

    public class AchievementDisplayEntry
    {
        public TiglUserAchievementDto Achievement { get; set; } = default!;

        public double RarityPercent { get; set; }

        public bool IsEarned { get; set; }
    }

    public class SeasonSummary
    {
        public TiglLeague League { get; set; }

        public int Season { get; set; }

        public int GamesPlayed { get; set; }

        public int Wins { get; set; }

        public double WinRate => GamesPlayed > 0 ? (double)Wins / GamesPlayed * 100 : 0;

        public List<TiglProfileGameDto> Games { get; set; } = new();
    }

    private GameRatingDeltas GetGameRatingDeltas(TiglProfileGameDto game)
    {
        if (Profile?.LeagueProfiles is null)
            return default;

        var lp = Profile.LeagueProfiles.FirstOrDefault(l => l.League == game.League);
        if (lp is null)
            return default;

        var asyncMatch = lp.AsyncMatchHistory.FirstOrDefault(m => m.EndTimestamp == game.EndTimestamp);
        var glickoMatch = lp.GlickoMatchHistory.FirstOrDefault(m => m.EndTimestamp == game.EndTimestamp);
        var tsMatch = lp.TrueSkillMatchHistory.FirstOrDefault(m => m.EndTimestamp == game.EndTimestamp);

        return new GameRatingDeltas(
            asyncMatch?.RatingChange ?? 0, asyncMatch is not null,
            glickoMatch?.RatingChange ?? 0, glickoMatch is not null,
            tsMatch?.MuChange ?? 0, tsMatch is not null);
    }

    private GameRatingDeltas GetSeasonRatingDeltas(SeasonSummary season)
    {
        double asyncSum = 0, glickoSum = 0, tsSum = 0;
        bool hasAsync = false, hasGlicko = false, hasTs = false;

        foreach (var game in season.Games)
        {
            var d = GetGameRatingDeltas(game);
            if (d.HasAsync) { asyncSum += d.AsyncDelta; hasAsync = true; }
            if (d.HasGlicko) { glickoSum += d.GlickoDelta; hasGlicko = true; }
            if (d.HasTrueSkill) { tsSum += d.TrueSkillDelta; hasTs = true; }
        }

        return new GameRatingDeltas(asyncSum, hasAsync, glickoSum, hasGlicko, tsSum, hasTs);
    }

    private SeasonRatingSummary GetSeasonRatingSummary(SeasonSummary season)
    {
        if (Profile?.LeagueProfiles is null)
            return default;

        var lp = Profile.LeagueProfiles.FirstOrDefault(l => l.League == season.League);
        if (lp is null)
            return default;

        var lastGame = season.Games.FirstOrDefault();
        if (lastGame is null)
            return default;

        var lastAsync = lp.AsyncMatchHistory.FirstOrDefault(m => m.EndTimestamp == lastGame.EndTimestamp);
        var lastGlicko = lp.GlickoMatchHistory.FirstOrDefault(m => m.EndTimestamp == lastGame.EndTimestamp);
        var lastTs = lp.TrueSkillMatchHistory.FirstOrDefault(m => m.EndTimestamp == lastGame.EndTimestamp);

        double asyncSum = 0, glickoSum = 0, tsSum = 0;
        bool hasAsync = false, hasGlicko = false, hasTs = false;

        foreach (var game in season.Games)
        {
            var d = GetGameRatingDeltas(game);
            if (d.HasAsync) { asyncSum += d.AsyncDelta; hasAsync = true; }
            if (d.HasGlicko) { glickoSum += d.GlickoDelta; hasGlicko = true; }
            if (d.HasTrueSkill) { tsSum += d.TrueSkillDelta; hasTs = true; }
        }

        return new SeasonRatingSummary(
            lastAsync?.RatingNew ?? 0, asyncSum, hasAsync,
            lastGlicko?.RatingNew ?? 0, glickoSum, hasGlicko,
            lastTs?.MuNew ?? 0, tsSum, hasTs);
    }

    private static string FormatDelta(double delta, string precision = "F1")
    {
        return delta >= 0 ? $"+{delta.ToString(precision)}" : delta.ToString(precision);
    }

    private static TextColor GetDeltaColor(double delta) =>
        delta >= 0 ? TextColor.Green : TextColor.Red;

    private static string GetDeltaArrow(double delta) =>
        delta >= 0 ? "\u25b2" : "\u25bc";

    private readonly record struct GameRatingDeltas(
        double AsyncDelta, bool HasAsync,
        double GlickoDelta, bool HasGlicko,
        double TrueSkillDelta, bool HasTrueSkill);

    private readonly record struct SeasonRatingSummary(
        double AsyncEnd, double AsyncDelta, bool HasAsync,
        double GlickoEnd, double GlickoDelta, bool HasGlicko,
        double TrueSkillEnd, double TrueSkillDelta, bool HasTrueSkill);

    private static string FormatRankUpDuration(long fromTimestamp, long toTimestamp)
    {
        if (fromTimestamp <= 0 || toTimestamp <= 0)
            return string.Empty;

        var from = DateTimeOffset.FromUnixTimeMilliseconds(fromTimestamp);
        var to = DateTimeOffset.FromUnixTimeMilliseconds(toTimestamp);
        var duration = to - from;

        if (duration.TotalDays < 1)
            return "<24h";

        var totalDays = (int)duration.TotalDays;
        if (totalDays >= 365)
        {
            var years = totalDays / 365;
            var remainingDays = totalDays % 365;
            if (remainingDays > 0)
                return $"{years} {(years == 1 ? "year" : "years")} {remainingDays} {(remainingDays == 1 ? "day" : "days")}";
            return $"{years} {(years == 1 ? "year" : "years")}";
        }

        return $"{totalDays} {(totalDays == 1 ? "day" : "days")}";
    }

    private static string GetRankUpTime(IList<RankHistoryDto> ranks, int index)
    {
        if (index >= ranks.Count - 1)
            return string.Empty;

        return FormatRankUpDuration(ranks[index + 1].AchievedAt, ranks[index].AchievedAt);
    }

    private string GetPrestigeRankUpTime(IList<PrestigeRankHistoryDto> prestiges, int index, TiglLeague league)
    {
        if (index < prestiges.Count - 1)
        {
            // Compute from previous prestige rank
            return FormatRankUpDuration(prestiges[index + 1].AchievedAt, prestiges[index].AchievedAt);
        }

        // First prestige rank — for ProphecyOfKings the Hero rank is achieved at the same time
        // as the 1st prestige rank, so compute from the Commander rank instead.
        var referenceRank = league == TiglLeague.ProphecyOfKings
            ? Profile?.RankHistory?
                .Where(r => r.League == league && r.Rank == TiglRankName.Commander)
                .OrderByDescending(r => r.AchievedAt)
                .FirstOrDefault()
            : Profile?.RankHistory?
                .Where(r => r.League == league)
                .OrderByDescending(r => r.AchievedAt)
                .FirstOrDefault();

        if (referenceRank is not null)
            return FormatRankUpDuration(referenceRank.AchievedAt, prestiges[index].AchievedAt);

        return string.Empty;
    }

    private string GetHeroIconPath(TiglFactionName faction) => PathProvider.GetLeaderIconPath(faction, LeaderType.Hero);

    private static string GetPrestigeDisplayText(PrestigeRankHistoryDto prestige)
    {
        var name = prestige.PrestigeRank.GetDisplayName();
        if (prestige.PrestigeRank is TiglPrestigeRank.GalacticThreat or TiglPrestigeRank.Tyrant && prestige.Level > 0)
        {
            var roman = prestige.Level switch
            {
                1 => "I",
                2 => "II",
                3 => "III",
                4 => "IV",
                5 => "V",
                _ => prestige.Level.ToString(),
            };
            return $"{name} {roman}";
        }

        return name;
    }
}
