namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class TiglFactionImg
{
    [Parameter]
    public TiglFactionName Faction { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    [Parameter]
    public int Width { get; set; } = 100;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetTiglFactionIconPath(Faction);
}