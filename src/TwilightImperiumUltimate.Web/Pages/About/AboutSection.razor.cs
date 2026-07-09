using Microsoft.AspNetCore.Components;

namespace TwilightImperiumUltimate.Web.Pages.About;

public partial class AboutSection
{
    /// <summary>
    /// Gets or sets the section heading text.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string HeadingText { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the section body content.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public MarkupString Content { get; set; }
}
