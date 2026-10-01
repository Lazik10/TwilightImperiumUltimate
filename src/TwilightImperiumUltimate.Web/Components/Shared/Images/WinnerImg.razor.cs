namespace TwilightImperiumUltimate.Web.Components.Shared.Images;

public partial class WinnerImg
{
    [Parameter]
    public int Height { get; set; } = 40;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetIconPath(IconType.Winner);
}
