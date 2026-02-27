using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Web.Models.Rankings;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class RanksGrid
{
    private TiglLeague _league = TiglLeague.ThundersEdge;
    private bool _loading = true;
    private List<TiglUserRanking> _shownRankings = new();
    private Dictionary<string, List<TiglUserRanking>> _groupedRankings = new();

    [Parameter]
    public IReadOnlyCollection<RankingsUserDto> Rankings { get; set; } = new List<RankingsUserDto>();

    protected override void OnParametersSet()
    {
        if (Rankings is not null)
        {
            _shownRankings = BuildRankings();

            if (_league == TiglLeague.ProphecyOfKings || _league == TiglLeague.ThundersEdge)
                _groupedRankings = GroupRankingsStandard(_shownRankings);
            else
                _groupedRankings = GroupRankingsFractured(_shownRankings);

            _loading = false;
        }
    }

    private Dictionary<string, List<TiglUserRanking>> GroupRankingsFractured(List<TiglUserRanking> list)
    {
        var result = new Dictionary<string, List<TiglUserRanking>>();

        var withPrestige = list.Where(x => x.HasPrestigeRank).ToList();
        var withoutPrestige = list.Where(x => !x.HasPrestigeRank).ToList();

        // Tyrant groups first: create a separate group for each PrestigeLevel ("Tyrant #X")
        var tyrants = withPrestige
            .Where(x => x.Prestige == TiglPrestigeRank.Tyrant)
            .ToList();

        var used = new HashSet<int>();

        foreach (var level in tyrants.Select(t => t.PrestigeLevel).Distinct().OrderByDescending(l => l))
        {
            var group = tyrants
                .Where(t => t.PrestigeLevel == level)
                .OrderByDescending(t => t.Rank)
                .ThenBy(t => t.GamesPlayed)
                .ThenBy(t => t.LastAchievedAt)
                .ToList();

            if (group.Count > 0)
            {
                foreach (var g in group)
                    used.Add(g.Id);

                // Key formatted as "Tyrant #X" where X is the PrestigeLevel
                result[$"Tyrant #{level}"] = group;
            }
        }

        // Helper utilities after initial Tyrant groups
        List<TiglUserRanking> ExcludeUsed(IEnumerable<TiglUserRanking> source) => source.Where(x => !used.Contains(x.Id)).ToList();
        void AddGroup(string key, IEnumerable<TiglUserRanking> items)
        {
            var listItems = ExcludeUsed(items);
            foreach (var i in listItems) used.Add(i.Id);
            if (listItems.Count > 0)
                result[key] = listItems;
        }

        AddGroup(Strings.Rankings_PrestigeRankCategory_Archon, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Archon)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_IxthLord, withoutPrestige
            .Where(x => x.Rank == TiglRankName.IxthLord)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_GeneSorcerer, withoutPrestige
            .Where(x => x.Rank == TiglRankName.GeneSorcerer)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Starlancer, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Starlancer)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Legionnaire, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Legionnaire)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Acolyte, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Acolyte)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Minister, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Thrall)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        return result;
    }

    private List<TiglUserRanking> BuildRankings()
    {
        var list = new List<TiglUserRanking>();

        foreach (var rankingDto in Rankings)
        {
            var leagueInfo = rankingDto.Leagues.FirstOrDefault(x => x.League == _league);

            if (leagueInfo != null)
            {
                var ranking = new TiglUserRanking()
                {
                    Id = rankingDto.TiglUserId,
                    TiglUserName = rankingDto.TiglUserName,
                    Prestige = leagueInfo.HighestPrestigeRank,
                    PrestigeLevel = leagueInfo.HighestPrestigeRankLevel,
                    Rank = leagueInfo.CurrentRank,
                    FactionPrestigeRankCount = leagueInfo.FactionPrestigeRankCount,
                    LastAchievedAt = leagueInfo.LastAchievedAt,
                    GamesPlayed = leagueInfo.GamesPlayed,
                };

                list.Add(ranking);
            }
        }

        return list;
    }

    private void ChangeLeague(TiglLeague league)
    {
        _league = league;
        _loading = true;
        StateHasChanged();

        _shownRankings = BuildRankings();

        if (_league == TiglLeague.ProphecyOfKings || _league == TiglLeague.ThundersEdge)
            _groupedRankings = GroupRankingsStandard(_shownRankings);
        else
            _groupedRankings = GroupRankingsFractured(_shownRankings);

        _loading = false;
        StateHasChanged();
    }

    private static Dictionary<string, List<TiglUserRanking>> GroupRankingsStandard(List<TiglUserRanking> list)
    {
        var result = new Dictionary<string, List<TiglUserRanking>>();

        var withPrestige = list.Where(x => x.HasPrestigeRank).ToList();
        var withoutPrestige = list.Where(x => !x.HasPrestigeRank).ToList();

        // PMBG group first
        var pmbg = withPrestige
            .Where(x => x.Prestige == TiglPrestigeRank.PaxMagnificaBellumGloriosum)
            .OrderBy(x => x.LastAchievedAt)
            .ToList();
        if (pmbg.Count > 0)
            result[Strings.Rankings_PrestigeRankCategory_Pmbg] = pmbg;

        // Track already grouped users to avoid duplicates across groups
        var used = new HashSet<int>(pmbg.Select(x => x.Id));
        List<TiglUserRanking> ExcludeUsed(IEnumerable<TiglUserRanking> source) => source.Where(x => !used.Contains(x.Id)).ToList();
        void AddGroup(string key, IEnumerable<TiglUserRanking> items)
        {
            var listItems = ExcludeUsed(items);
            foreach (var i in listItems) used.Add(i.Id);
            if (listItems.Count > 0)
                result[key] = listItems;
        }

        // Define prestige buckets (Min..Max inclusive) with localized titles
        var prestigeBuckets = new (string Title, int Min, int Max)[]
        {
            (Strings.Rankings_PrestigeRankCategory_GalacticThreatFive, 25, 31),
            (Strings.Rankings_PrestigeRankCategory_GalacticThreatFour, 20, 24),
            (Strings.Rankings_PrestigeRankCategory_GalacticThreatThree, 15, 19),
            (Strings.Rankings_PrestigeRankCategory_GalacticThreatTwo, 10, 14),
            (Strings.Rankings_PrestigeRankCategory_GalacticThreatOne, 5, 9),
        };

        foreach (var (title, min, max) in prestigeBuckets)
        {
            AddGroup(
                title,
                withPrestige
                    .Where(x => x.Prestige != TiglPrestigeRank.PaxMagnificaBellumGloriosum
                             && x.FactionPrestigeRankCount >= min && x.FactionPrestigeRankCount <= max)
                    .OrderByDescending(x => x.FactionPrestigeRankCount)
                    .ThenBy(x => x.GamesPlayed)
                    .ThenBy(x => x.LastAchievedAt));
        }

        // Hero (non-prestige)
        AddGroup(Strings.Rankings_PrestigeRankCategory_Hero, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Hero)
            .OrderByDescending(x => x.FactionPrestigeRankCount)
            .ThenBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        // Commander, Agent, Minister (non-prestige) as separate groups
        AddGroup(Strings.Rankings_PrestigeRankCategory_Commander, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Commander)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Agent, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Agent)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        AddGroup(Strings.Rankings_PrestigeRankCategory_Minister, withoutPrestige
            .Where(x => x.Rank == TiglRankName.Minister)
            .OrderBy(x => x.GamesPlayed)
            .ThenBy(x => x.LastAchievedAt));

        return result;
    }
}