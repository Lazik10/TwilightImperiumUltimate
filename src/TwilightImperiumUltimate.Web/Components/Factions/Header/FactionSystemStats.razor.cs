namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionSystemStats
{
    private MarkupString _factionSystemStats = new MarkupString(string.Empty);

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        _factionSystemStats = new MarkupString(FactionProvider.CurrentFactionName.GetFactionUIText(FactionResourceType.SystemStats));
    }
}
