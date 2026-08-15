namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionHeader
{
    private FactionName FactionName => FactionProvider.CurrentFactionName;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;
}
