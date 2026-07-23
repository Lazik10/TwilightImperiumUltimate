namespace TwilightImperiumUltimate.Web.Models.Website;

/// <summary>
/// Create form model for the admin Website editor (add-only -- existing websites are removed and
/// re-added rather than edited in place, matching the "Other Websites" list's simple curated-list
/// nature).
/// </summary>
public class WebsiteFormModel
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string WebsitePath { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the picked preview image's bytes, read client-side from an <c>InputFile</c>.
    /// Optional -- when left unset, the new website falls back to the legacy built-in image
    /// lookup by title (so existing curated titles keep working without an upload).
    /// </summary>
    public byte[]? ImageData { get; set; }

    public string? ImageContentType { get; set; }

    /// <summary>
    /// Gets or sets the picked image file's name, shown next to the file picker as confirmation
    /// that a file was selected. Purely for display -- not sent to the API.
    /// </summary>
    public string? ImageFileName { get; set; }
}
