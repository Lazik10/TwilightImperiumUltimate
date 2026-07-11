namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

/// <summary>
/// Renders a responsive, semantic heading element (h1-h6) with fluid typography.
/// The heading always wraps its text so it stays readable on any screen size.
/// Always renders with the "handel white shadow" classes applied; pass additional
/// classes via <see cref="CssClass"/> rather than repeating those on every call site.
/// </summary>
public partial class ResponsiveHeading
{
    /// <summary>
    /// Gets or sets the heading text to display.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the heading level (1-6). Determines the rendered element (h1-h6).
    /// Values outside the range are clamped.
    /// </summary>
    [Parameter]
    public int Level { get; set; } = 1;

    /// <summary>
    /// Gets or sets the font-size token (xs, sm, base, md, lg, xl, 2xl, 3xl).
    /// When null, the font size is derived automatically from <see cref="Level"/>
    /// (h1 largest down to h6 smallest). Set explicitly to override that default.
    /// </summary>
    [Parameter]
    public string? FontSize { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the heading text is centered.
    /// </summary>
    [Parameter]
    public bool CenterText { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes to apply to the heading.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles to apply to the heading.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    private int HeadingLevel => Math.Clamp(Level, 1, 6);

    private string ComputedCssClass =>
        $"responsive-heading handel white shadow{(CenterText ? " centered-text" : string.Empty)} {CssClass}".Trim();

    private string ComputedStyle =>
        $"font-size: {ResolveFontSize(FontSize ?? GetDefaultFontSizeForLevel())}; {Style}";

    private static string ResolveFontSize(string fontSize) => fontSize switch
    {
        "xs" => "var(--font-size-xs)",
        "sm" => "var(--font-size-sm)",
        "base" => "var(--font-size-base)",
        "md" => "var(--font-size-md)",
        "lg" => "var(--font-size-lg)",
        "xl" => "var(--font-size-xl)",
        "2xl" => "var(--font-size-2xl)",
        "3xl" => "var(--font-size-3xl)",
        _ => "var(--font-size-2xl)",
    };

    private string GetDefaultFontSizeForLevel() => HeadingLevel switch
    {
        1 => "3xl",
        2 => "2xl",
        3 => "xl",
        4 => "lg",
        5 => "md",
        _ => "base",
    };
}
