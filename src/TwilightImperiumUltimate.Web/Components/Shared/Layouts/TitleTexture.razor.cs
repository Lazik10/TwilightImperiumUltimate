namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class TitleTexture
{
    [Parameter]
    public int Width { get; set; } = 15;

    [Parameter]
    public Texture Texture { get; set; }

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetTexturePath(Texture);
}