namespace TwilightImperiumUltimate.Web.Components.TiglProfile;
public partial class TiglSeasonHistoryTable
{
    [Parameter] public IReadOnlyList<TiglProfileSeasonSummary> Seasons { get; set; } = [];
    [Parameter] public EventCallback<int> OnGameSelected { get; set; }
    private readonly HashSet<int> _expandedSeasons = [];
    private bool IsExpanded(TiglProfileSeasonSummary season) => _expandedSeasons.Contains(season.Season);
    private void Toggle(TiglProfileSeasonSummary season) { if (!_expandedSeasons.Add(season.Season)) _expandedSeasons.Remove(season.Season); }
}
