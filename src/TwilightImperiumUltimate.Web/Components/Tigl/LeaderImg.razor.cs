namespace TwilightImperiumUltimate.Web.Components.Tigl;

public partial class LeaderImg
{
    [Parameter]
    public TiglFactionName Faction { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetLeaderIconPath(Faction, LeaderType.Hero);
}