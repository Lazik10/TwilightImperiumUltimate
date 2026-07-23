namespace TwilightImperiumUltimate.Web.Components.Websites;

public partial class WebsiteCard : TwilightImperiumBaseComponent
{
    [Parameter]
    public string Title { get; set; } = string.Empty;

    [Parameter]
    public string Description { get; set; } = string.Empty;

    [Parameter]
    public string WebsitePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the uploaded preview image bytes for a website added at runtime through the
    /// admin editor. When set (together with <see cref="ImageContentType"/>), takes precedence
    /// over the legacy built-in image lookup by <see cref="Title"/>.
    /// </summary>
    [Parameter]
    public byte[]? ImageData { get; set; }

    [Parameter]
    public string? ImageContentType { get; set; }

    private string GetWebsiteIconPath()
    {
        if (ImageData is { Length: > 0 } && !string.IsNullOrWhiteSpace(ImageContentType))
            return $"data:{ImageContentType};base64,{Convert.ToBase64String(ImageData)}";

        return PathProvider.GetWebsitePreviewImagePath(Title);
    }
}
