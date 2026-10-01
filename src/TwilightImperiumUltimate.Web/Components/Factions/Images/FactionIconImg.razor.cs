namespace TwilightImperiumUltimate.Web.Components.Factions.Images;

public partial class FactionIconImg
{
    [Parameter]
    public FactionName FactionName { get; set; }

    [Parameter]
    public string Width { get; set; } = "100%";

    [Parameter]
    public string MaxHeight { get; set; } = "clamp(32px, 10vw, 70px)";

    [Inject]
    private IPathProvider PathProvider { get; set; } = default!;

    private string ImgPath() => PathProvider.GetFactionIconPath(FactionName);

    private string GetInlineStyle()
    {
        var width = string.IsNullOrWhiteSpace(Width)
            ? "100%"
            : Width;

        var maxHeight = string.IsNullOrWhiteSpace(MaxHeight)
            ? "none"
            : MaxHeight;

        return $"width: {width}; max-height: {maxHeight};";
    }
}
