namespace TwilightImperiumUltimate.Web.Components.Factions.MainSection;

public partial class FactionHomeSystem
{
    [Parameter]
    public string Title { get; set; } = "Home System";

    [Parameter]
    [EditorRequired]
    public SystemTileModel SystemTile { get; set; } = new();

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;
}
