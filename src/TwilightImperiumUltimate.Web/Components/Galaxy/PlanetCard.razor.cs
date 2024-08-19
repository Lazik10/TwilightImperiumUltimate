namespace TwilightImperiumUltimate.Web.Components.Galaxy;

public partial class PlanetCard
{
    [Parameter]
    public string Name { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string GetPlanetImagePath()
    {
        return PathProvider.GetPlanetImagePath(Name);
    }
}