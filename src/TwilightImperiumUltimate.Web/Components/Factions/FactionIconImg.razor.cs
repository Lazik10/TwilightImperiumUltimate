namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class FactionIconImg
{
    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetFactionIconPath(FactionName);
}