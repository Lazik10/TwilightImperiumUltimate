using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.DTOs.Tigl;

namespace TwilightImperiumUltimate.Web.Components.TiglProfile;

public partial class TiglProfileGrid
{
    private bool _showAllAchievements;
    public TiglProfileCategory CurrentCategory { get; set; } = TiglProfileCategory.ThundersEdge;

    [CascadingParameter(Name = "TiglPlayerProfile")]
    public TiglPlayerProfileDto Profile { get; set; } = default!;

    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IPathProvider PathProvider { get; set; } = default!;

    private TiglLeague SelectedLeague => CurrentCategory switch
    {
        TiglProfileCategory.Fractured => TiglLeague.Fractured,
        TiglProfileCategory.ProphecyOfKings => TiglLeague.ProphecyOfKings,
        _ => TiglLeague.ThundersEdge,
    };

    private TiglLeagueProfileDto? SelectedLeagueProfile => Profile?.LeagueProfiles.FirstOrDefault(profile => profile.League == SelectedLeague);
    private PrestigeRankHistoryDto? SelectedPrestige => Profile?.PrestigeRankHistory.Where(prestige => prestige.League == SelectedLeague).OrderByDescending(prestige => prestige.Level).ThenByDescending(prestige => prestige.AchievedAt).FirstOrDefault();
    private IReadOnlyList<RankHistoryDto> FilteredRankHistory => Profile?.RankHistory.Where(rank => rank.League == SelectedLeague).OrderByDescending(rank => rank.AchievedAt).ToList() ?? [];
    private IReadOnlyList<PrestigeRankHistoryDto> FilteredPrestigeRanks => Profile?.PrestigeRankHistory.Where(prestige => prestige.League == SelectedLeague).OrderByDescending(prestige => prestige.AchievedAt).ToList() ?? [];
    private IReadOnlyList<TiglProfileGameDto> FilteredGameHistory => Profile?.GameHistory.Where(game => game.League == SelectedLeague).OrderByDescending(game => game.EndTimestamp).ToList() ?? [];
    private IReadOnlyList<TiglTopOpponentDto> FilteredTopOpponents => Profile?.TopOpponents.Where(opponent => opponent.League == SelectedLeague).OrderByDescending(opponent => opponent.GamesPlayed).Take(20).ToList() ?? [];
    private IReadOnlyList<TiglProfileFactionStatsDto> FilteredFactionStats => Enum.GetValues<TiglFactionName>()
        .Where(faction => faction != TiglFactionName.None)
        .Select(faction => Profile.FactionStats.FirstOrDefault(stats => stats.League == SelectedLeague && stats.Faction == faction)
            ?? new TiglProfileFactionStatsDto { Faction = faction, League = SelectedLeague })
        .ToList();
    private IReadOnlyList<TiglProfileSeasonSummary> FilteredSeasons => FilteredGameHistory.GroupBy(game => game.Season).OrderByDescending(group => group.Key).Select(group => new TiglProfileSeasonSummary(group.Key, group.ToList())).ToList();
    private IReadOnlyList<AchievementDisplayEntry> DisplayedAchievements => GetDisplayedAchievements();

    private void OnCategoryChanged(TiglProfileCategory category) { CurrentCategory = category == TiglProfileCategory.All ? TiglProfileCategory.ThundersEdge : category; }
    private void NavigateToGameDetail(int matchReportId) { var returnUrl = $"{Web.Pages.Pages.TiglPlayerProfile}?playerId={Profile.TiglUserId}"; NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglGameDetail}?id={matchReportId}&returnUrl={Uri.EscapeDataString(returnUrl)}"); }
    private void NavigateToPlayerProfile(int tiglUserId) => NavigationManager.NavigateTo($"{Web.Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={Uri.EscapeDataString(NavigationManager.Uri)}");
    private double GetRarityPercent(AchievementName name) => Profile is null || Profile.TotalTiglUsers == 0 ? 0 : Profile.AchievementPlayerCounts.TryGetValue(name.ToString(), out var count) ? (double)count / Profile.TotalTiglUsers * 100 : 0;

    private IReadOnlyList<AchievementDisplayEntry> GetDisplayedAchievements()
    {
        if (Profile is null) return [];
        var earned = Profile.Achievements.OrderBy(achievement => GetRarityPercent(achievement.AchievementName)).Select(achievement => new AchievementDisplayEntry(achievement, GetRarityPercent(achievement.AchievementName), true)).ToList();
        if (!_showAllAchievements) return earned;
        var earnedNames = earned.Select(entry => entry.Achievement.AchievementName).ToHashSet();
        earned.AddRange(Enum.GetValues<AchievementName>().Where(name => !earnedNames.Contains(name)).Select(name => new AchievementDisplayEntry(new TiglUserAchievementDto { AchievementName = name }, GetRarityPercent(name), false)).OrderBy(entry => entry.RarityPercent));
        return earned;
    }

    private void OnShowAllAchievementsChanged(ChangeEventArgs args) => _showAllAchievements = (bool)(args.Value ?? false);
    private sealed record AchievementDisplayEntry(TiglUserAchievementDto Achievement, double RarityPercent, bool IsEarned);
}
