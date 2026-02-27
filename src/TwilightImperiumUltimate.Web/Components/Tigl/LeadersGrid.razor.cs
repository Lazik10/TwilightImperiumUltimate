using TwilightImperiumUltimate.Contracts.DTOs.Rankings;
using TwilightImperiumUltimate.Contracts.Enums;

namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class LeadersGrid
{
    private TiglLeague _league = TiglLeague.ThundersEdge;
    private bool _loading = true;
    private Dictionary<TiglLeague, List<RankingsLeaderDto>> _grouped = new Dictionary<TiglLeague, List<RankingsLeaderDto>>();

    [Parameter]
    public IReadOnlyCollection<RankingsLeaderDto> Leaders { get; set; } = new List<RankingsLeaderDto>();

    [Inject]
    private NavigationManager NavigationManager { get; set; } = default!;

    private List<RankingsLeaderDto> CurrentLeagueLeaders => _grouped.TryGetValue(_league, out var list) ? list : new List<RankingsLeaderDto>();

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

    private void ChangeLeague(TiglLeague league)
    {
        _league = league;
        StateHasChanged();
    }

    private void NavigateToProfile(int tiglUserId)
    {
        if (tiglUserId == 0)
            return;

        var returnUrl = Uri.EscapeDataString(Pages.Pages.TiglLeaders);
        NavigationManager.NavigateTo($"{Pages.Pages.TiglPlayerProfile}?playerId={tiglUserId}&returnUrl={returnUrl}");
    }
}