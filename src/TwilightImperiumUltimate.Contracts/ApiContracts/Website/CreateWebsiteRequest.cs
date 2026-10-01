namespace TwilightImperiumUltimate.Contracts.ApiContracts.Website;

public class CreateWebsiteRequest
{
    public required string Title { get; set; }

    public required string Description { get; set; }

    public required string WebsitePath { get; set; }

    /// <summary>
    /// Gets or sets the uploaded preview image bytes, stored directly in the database so a new
    /// website (and its image) can be added at runtime without requiring a new deployment.
    /// Optional -- when omitted, the client falls back to the legacy built-in image lookup by title.
    /// </summary>
    public byte[]? ImageData { get; set; }

    /// <summary>
    /// Gets or sets the uploaded image's MIME type (e.g. "image/webp"), required alongside
    /// <see cref="ImageData"/> to render it correctly.
    /// </summary>
    public string? ImageContentType { get; set; }
}
