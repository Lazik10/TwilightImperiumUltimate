using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class TitleLine
{
    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Parameter]
    public int TransformX { get; set; } = 0;

    [Parameter]
    public int TransformY { get; set; } = 0;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetTexturePath(Texture.Line);
}