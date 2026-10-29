using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;
using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileGrid
{
    private bool _showAllAchievements;
    private bool _areFactionFiltersVisible;
    private FactionStatisticsFilter _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;

    [Parameter]
    public bool IsLoading { get; set; }

    public TiglProfileCategory CurrentCategory { get; set; } = TiglProfileCategory.ThundersEdge;

    [CascadingParameter(Name = "TiglPlayerProfile")]
    public TiglPlayerProfileDto Profile { get; set; } = default!;

    private static IReadOnlyList<KeyValuePair<TiglProfileCategory, string>> Options =>
    [
        new(TiglProfileCategory.ThundersEdge, "Standard"),
        new(TiglProfileCategory.Fractured, "Fractured"),
        new(TiglProfileCategory.ProphecyOfKings, "Legacy"),
    ];

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private TiglLeague SelectedLeague => CurrentCategory switch
    {
        TiglProfileCategory.Fractured => TiglLeague.Fractured,
        TiglProfileCategory.ProphecyOfKings => TiglLeague.ProphecyOfKings,
        _ => TiglLeague.ThundersEdge,
    };

    private FactionStatisticsFilter EffectiveFactionStatisticsFilter => SelectedLeague == TiglLeague.Fractured
        ? _selectedFactionStatisticsFilter
        : FactionStatisticsFilter.Official;

    private bool AreFactionFiltersApplied => SelectedLeague == TiglLeague.Fractured
        && _selectedFactionStatisticsFilter != FactionStatisticsFilter.Official;

    private string FilterButtonLabel => (_areFactionFiltersVisible ? "Hide faction filters" : "Show faction filters")
        + (AreFactionFiltersApplied ? ", filters active" : string.Empty);

    private string FilterIconPath => PathProvider.GetIconPath(_areFactionFiltersVisible || AreFactionFiltersApplied ? IconType.FilterClicked : IconType.Filter);

    private TiglLeagueProfileDto? SelectedLeagueProfile => Profile?.LeagueProfiles.FirstOrDefault(profile => profile.League == SelectedLeague);

    private PrestigeRankHistoryDto? SelectedPrestige => Profile?.PrestigeRankHistory
        .Where(prestige => prestige.League == SelectedLeague && prestige.PrestigeRank.IsLeagueMilestone(SelectedLeague))
        .OrderByDescending(prestige => prestige.Level)
        .ThenByDescending(prestige => prestige.AchievedAt)
        .FirstOrDefault();

    private static List<KeyValuePair<FactionStatisticsFilter, string>> GetFactionFilterOptions() => Enum.GetValues<FactionStatisticsFilter>()
        .Select(filter => new KeyValuePair<FactionStatisticsFilter, string>(filter, filter.GetDisplayName()))
        .ToList();

    private List<RankHistoryDto> GetFilteredRankHistory() => Profile?.RankHistory.Where(rank => rank.League == SelectedLeague).OrderByDescending(rank => rank.AchievedAt).ToList() ?? [];

    private List<PrestigeRankHistoryDto> GetFilteredPrestigeRanks() => Profile?.PrestigeRankHistory
        .Where(prestige => prestige.League == SelectedLeague)
        .OrderByDescending(prestige => prestige.AchievedAt)
        .ThenByDescending(prestige => prestige.Id)
        .ToList() ?? [];

    private List<TiglProfileGameDto> GetFilteredGameHistory() => Profile?.GameHistory.Where(game => game.League == SelectedLeague).OrderByDescending(game => game.EndTimestamp).ToList() ?? [];

    private List<TiglTopOpponentDto> GetFilteredTopOpponents() => Profile?.TopOpponents.Where(opponent => opponent.League == SelectedLeague).OrderByDescending(opponent => opponent.GamesPlayed).Take(20).ToList() ?? [];

    private List<TiglProfileFactionStatsDto> GetFilteredFactionStats() => Enum.GetValues<TiglFactionName>()
        .Where(faction => faction != TiglFactionName.None)
        .Where(IsFactionVisible)
        .Select(faction => Profile.FactionStats.FirstOrDefault(stats => stats.League == SelectedLeague && stats.Faction == faction)
            ?? new TiglProfileFactionStatsDto { Faction = faction, League = SelectedLeague })
        .ToList();

    private List<TiglProfileSeasonSummary> GetFilteredSeasons() => GetFilteredGameHistory().GroupBy(game => game.Season).OrderByDescending(group => group.Key).Select(group => new TiglProfileSeasonSummary(group.Key, group.ToList())).ToList();

    private void OnCategoryChanged(TiglProfileCategory category)
    {
        CurrentCategory = category == TiglProfileCategory.All ? TiglProfileCategory.ThundersEdge : category;
        if (SelectedLeague != TiglLeague.Fractured)
        {
            _selectedFactionStatisticsFilter = FactionStatisticsFilter.Official;
            _areFactionFiltersVisible = false;
        }
    }

    private void OnFactionStatisticsFilterChanged(FactionStatisticsFilter filter) => _selectedFactionStatisticsFilter = filter;

    private void ToggleFactionFilters() => _areFactionFiltersVisible = !_areFactionFiltersVisible;

    private void NavigateToGameDetail(int matchReportId) => NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglGameDetail}?id={matchReportId}");

    private void NavigateToPlayerProfile(int tiglUserId) => NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}");

    private double GetRarityPercent(AchievementName name)
    {
        if (Profile is null || Profile.TotalTiglUsers == 0)
            return 0;

        return Profile.AchievementPlayerCounts.TryGetValue(name.ToString(), out var count)
            ? (double)count / Profile.TotalTiglUsers * 100
            : 0;
    }

    private List<AchievementDisplayEntry> GetDisplayedAchievements()
    {
        if (Profile is null) return [];
        var earned = Profile.Achievements.OrderBy(achievement => GetRarityPercent(achievement.AchievementName)).Select(achievement => new AchievementDisplayEntry(achievement, GetRarityPercent(achievement.AchievementName), true)).ToList();
        if (!_showAllAchievements) return earned;
        var earnedNames = earned.Select(entry => entry.Achievement.AchievementName).ToHashSet();
        earned.AddRange(Enum.GetValues<AchievementName>().Where(name => !earnedNames.Contains(name)).Select(name => new AchievementDisplayEntry(new TiglUserAchievementDto { AchievementName = name }, GetRarityPercent(name), false)).OrderBy(entry => entry.RarityPercent));
        return earned;
    }

    private void OnShowAllAchievementsChanged(bool isChecked) => _showAllAchievements = isChecked;

    private bool IsFactionVisible(TiglFactionName faction) => EffectiveFactionStatisticsFilter switch
    {
        FactionStatisticsFilter.TwilightsFall => faction is >= TiglFactionName.TheRubyMonarch and <= TiglFactionName.ASickeningLurch,
        FactionStatisticsFilter.DiscordantStars => faction is >= TiglFactionName.TheAugursOfIlyxum and <= TiglFactionName.TheZelianPurifier,
        FactionStatisticsFilter.BlueReverie => faction is >= TiglFactionName.AtokeraLegacy and <= TiglFactionName.XinCourt,
        FactionStatisticsFilter.WhispersFromTheVoid => faction is >= TiglFactionName.TheArvaxiCovenant and <= TiglFactionName.TheZephyrionPursuit,
        FactionStatisticsFilter.Others => faction is >= TiglFactionName.Franken and <= TiglFactionName.Homebrew,
        _ => faction is >= TiglFactionName.TheArborec and <= TiglFactionName.TheRalNelConsortium,
    };

    private sealed record AchievementDisplayEntry(TiglUserAchievementDto Achievement, double RarityPercent, bool IsEarned);
}
