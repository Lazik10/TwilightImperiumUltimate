namespace TwilightImperiumUltimate.Web.Components.Factions.Header;

public partial class FactionImage
{
    public FactionName FactionName => FactionProvider.CurrentFactionName;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    [Inject]
    private IFactionProvider FactionProvider { get; set; } = default!;
}
