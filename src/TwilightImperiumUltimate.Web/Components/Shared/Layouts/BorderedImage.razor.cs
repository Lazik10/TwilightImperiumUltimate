namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

public partial class BorderedImage
{
    [Parameter]
    public string ImagePath { get; set; } = string.Empty;

    [Parameter]
    public int MaxWidth { get; set; } = 100;

    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets the accessible alt text for the image. Leave empty (the default) when the
    /// image is purely decorative or already described by surrounding text -- screen readers
    /// then skip it instead of announcing the raw <see cref="ImagePath"/>.
    /// </summary>
    [Parameter]
    public string AltText { get; set; } = string.Empty;
}
