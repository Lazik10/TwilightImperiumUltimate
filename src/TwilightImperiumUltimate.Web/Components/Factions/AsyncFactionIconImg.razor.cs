namespace TwilightImperiumUltimate.Web.Components.Factions;

public partial class AsyncFactionIconImg
{
    [Parameter]
    public AsyncFactionName FactionName { get; set; }

    [Parameter]
    public int Height { get; set; } = 40;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetAsyncFactionIconPath(FactionName);
}