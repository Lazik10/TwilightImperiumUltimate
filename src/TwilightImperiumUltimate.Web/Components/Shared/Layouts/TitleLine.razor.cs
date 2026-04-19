using Microsoft.AspNetCore.Components;
using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.Path;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class TitleLine
{
    [Parameter]
    public int Width { get; set; } = 100;

    [Parameter]
    public string WidthCss { get; set; } = string.Empty;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Parameter]
    public string MaxWidthCss { get; set; } = string.Empty;

    [Parameter]
    public int TransformX { get; set; } = 0;

    [Parameter]
    public string TransformXCss { get; set; } = string.Empty;

    [Parameter]
    public int TransformY { get; set; } = 0;

    [Parameter]
    public string TransformYCss { get; set; } = string.Empty;

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetTexturePath(Texture.Line);

    private string GetWidth() => string.IsNullOrWhiteSpace(WidthCss) ? $"{Width}%" : WidthCss;

    private string GetMaxWidth() => string.IsNullOrWhiteSpace(MaxWidthCss) ? $"{MaxWidth}%" : MaxWidthCss;

    private string GetTransformX() => string.IsNullOrWhiteSpace(TransformXCss) ? $"{TransformX}%" : TransformXCss;

    private string GetTransformY() => string.IsNullOrWhiteSpace(TransformYCss) ? $"{TransformY}%" : TransformYCss;
}