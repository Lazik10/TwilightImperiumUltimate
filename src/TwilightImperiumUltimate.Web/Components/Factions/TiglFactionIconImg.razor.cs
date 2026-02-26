namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class TiglFactionIconImg
{
    [Parameter]
    public TiglFactionName FactionName { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetTiglFactionIconPath(FactionName);
}
