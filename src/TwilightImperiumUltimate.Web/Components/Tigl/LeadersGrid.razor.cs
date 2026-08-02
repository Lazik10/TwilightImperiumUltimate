using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.Enums;
using TwilightImperiumUltimate.Web.Helpers.Time;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class LeadersGrid
{
    private TiglLeague _league = TiglLeague.ThundersEdge;
    private bool _loading = true;
    private Dictionary<TiglLeague, List<RankingsLeaderDto>> _grouped = new Dictionary<TiglLeague, List<RankingsLeaderDto>>();

    [Parameter]
    public IReadOnlyCollection<RankingsLeaderDto> Leaders { get; set; } = new List<RankingsLeaderDto>();

    private List<RankingsLeaderDto> CurrentLeagueLeaders => _grouped.TryGetValue(_league, out var list) ? list : new List<RankingsLeaderDto>();

    private string ActiveTabId => _league == TiglLeague.Fractured ? "leaders-tab-fractured" : "leaders-tab-standard";

    protected override void OnParametersSet()
    {
        if (Leaders is null)
        {
            _grouped = new Dictionary<TiglLeague, List<RankingsLeaderDto>>();
            return;
        }

        _grouped = Leaders
            .Where(x => x.Name != TiglPrestigeRank.TwilightsFall)
            .GroupBy(l => l.League)
            .ToDictionary(g => g.Key, g => g
                .OrderBy(x => x.Faction)
                .ToList());

        _loading = false;
    }

    private static long GetCurrentDurationMilliseconds(RankingsLeaderDto leader)
    {
        if (leader.LastUpdate is null || leader.LastUpdate == 0 || leader.ChangeCount == 0)
            return 0;

        var startDate = DateTimeOffset.FromUnixTimeMilliseconds(leader.LastUpdate.Value);
        var endDate = DateTimeOffset.Now;
        var duration = endDate - startDate;

        return (long)duration.TotalMilliseconds;
    }

    private static string GetDurationTime(RankingsLeaderDto leader)
    {
        if (leader.LastUpdate is null || leader.LastUpdate == 0 || leader.ChangeCount == 0)
            return string.Empty;

        var startDate = DateTimeOffset.FromUnixTimeMilliseconds(leader.LastUpdate.Value);
        var endDate = DateTimeOffset.Now;
        var duration = endDate - startDate;

        return duration.FormaToDays();
    }

    private static string GetShortestDurationTime(RankingsLeaderDto leader)
    {
        if (leader.ShortestDuration is null || leader.ShortestDuration.Value == 0)
        {
            return leader.LastUpdate is null || leader.LastUpdate.Value == 0
                ? string.Empty
                : GetDurationTime(leader);
        }

        return TimeSpan.FromMilliseconds(leader.ShortestDuration.Value).FormaToDays();
    }

    private static string GetShortestHolderName(RankingsLeaderDto leader)
    {
        if (!string.IsNullOrWhiteSpace(leader.ShortestHolderName))
            return leader.ShortestHolderName;

        return leader.ShortestDuration is null || leader.ShortestDuration.Value == 0
            ? leader.UserName
            : string.Empty;
    }

    private static string GetLongestDurationTime(RankingsLeaderDto leader)
    {
        var currentDuration = GetCurrentDurationMilliseconds(leader);
        var longestDuration = leader.LongestDuration ?? 0;

        if (currentDuration > longestDuration)
            return TimeSpan.FromMilliseconds(currentDuration).FormaToDays();

        if (longestDuration == 0)
        {
            return leader.LastUpdate is null || leader.LastUpdate.Value == 0
                ? string.Empty
                : GetDurationTime(leader);
        }

        return TimeSpan.FromMilliseconds(longestDuration).FormaToDays();
    }

    private static string GetLongestHolderName(RankingsLeaderDto leader)
    {
        var currentDuration = GetCurrentDurationMilliseconds(leader);
        var longestDuration = leader.LongestDuration ?? 0;

        if (currentDuration > longestDuration)
            return leader.UserName;

        if (!string.IsNullOrWhiteSpace(leader.LongestHolderName))
            return leader.LongestHolderName;

        return longestDuration == 0 ? leader.UserName : string.Empty;
    }

    private static string? GetProfileUrl(int tiglUserId)
    {
        if (tiglUserId == 0)
            return null;

        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglLeaders);
        return $"{Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}";
    }

    private void ChangeLeague(TiglLeague league)
    {
        _league = league;
        StateHasChanged();
    }
}
