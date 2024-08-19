namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class BorderedImage
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Parameter]
    public int Width { get; set; } = 100;
}