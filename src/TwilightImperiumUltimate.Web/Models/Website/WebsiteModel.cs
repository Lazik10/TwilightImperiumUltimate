namespace TwilightImperiumUltimate.Web.Models.Website;

public class WebsiteModel
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string WebsitePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the uploaded preview image bytes. Null for websites still using the legacy
    /// built-in image lookup by title (<see cref="Services.Path.IPathProvider.GetWebsitePreviewImagePath"/>).
    /// </summary>
    public byte[]? ImageData { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of <see cref="ImageData"/>. Null when <see cref="ImageData"/> is null.
    /// </summary>
    public string? ImageContentType { get; set; }
}
