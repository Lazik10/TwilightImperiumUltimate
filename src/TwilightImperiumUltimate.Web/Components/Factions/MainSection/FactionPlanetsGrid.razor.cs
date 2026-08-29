namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionPlanetsGrid
{
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<PlanetModel> Planets { get; set; } = [];

    private int DesktopColumns => Math.Clamp(Planets.Count, 1, 3);

    private int TabletColumns => Planets.Count <= 2 ? Math.Max(Planets.Count, 1) : 1;

    private int MobileColumns => TabletColumns;
}
