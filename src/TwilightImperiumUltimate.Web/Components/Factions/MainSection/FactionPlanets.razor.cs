namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionPlanets
{
    [Parameter]
    public string Title { get; set; } = "Planets";

    [Parameter]
    [EditorRequired]
    public IReadOnlyList<PlanetModel> Planets { get; set; } = [];
}
