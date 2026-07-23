using TwilightImperiumUltimate.Core.Interfaces;

namespace TwilightImperiumUltimate.Core.Entities.Website;

public class Website : IEntity
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string WebsitePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the uploaded preview image bytes. Stored directly in the database (rather than
    /// on disk) so a new website's image survives redeployments without needing to be published as
    /// a static resource. Null for websites still using the legacy built-in image lookup by title.
    /// </summary>
    public byte[]? ImageData { get; set; }

    /// <summary>
    /// Gets or sets the MIME type of <see cref="ImageData"/> (e.g. "image/webp"). Null when
    /// <see cref="ImageData"/> is null.
    /// </summary>
    public string? ImageContentType { get; set; }
}
