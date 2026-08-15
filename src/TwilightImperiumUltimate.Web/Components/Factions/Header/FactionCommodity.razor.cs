namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionCommodity
{
    private int _commodities;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    protected override void OnInitialized()
    {
        var faction = FactionProvider.CurrentFaction;
        _commodities = faction?.Commodities ?? 0;
    }
}
