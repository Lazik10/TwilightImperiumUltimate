namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

/// <summary>
/// Renders a responsive, semantic heading element (h1-h6) with fluid typography.
/// The heading always wraps its text so it stays readable on any screen size.
/// Always renders with the "handel white shadow" classes applied; pass additional
/// classes via <see cref="CssClass"/> rather than repeating those on every call site.
/// Applies a default bottom margin that decreases as the level increases (mb-xl for
/// level 1 down to mb-0 for level 6) unless <see cref="CssClass"/> already supplies
/// its own "mb-" class.
/// </summary>
public partial class ResponsiveHeading
{
    /// <summary>
    /// Gets or sets the heading text to display.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    [Parameter]
    public int Width { get; set; } = 100;

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
    /// When null, defaults to <see langword="true"/> for level 1 headings and
    /// <see langword="false"/> for every other level. Set explicitly to override.
    /// </summary>
    [Parameter]
    public bool? CenterText { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the heading text is prevented from wrapping
    /// onto multiple lines. Defaults to <see langword="false"/> (normal wrapping behavior).
    /// </summary>
    [Parameter]
    public bool NoWrap { get; set; }

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

    private bool EffectiveCenterText => CenterText ?? HeadingLevel == 1;

    private string ComputedCssClass
    {
        get
        {
            var marginBottomClass = CssClass.Contains("mb-", StringComparison.Ordinal)
                ? string.Empty
                : GetDefaultMarginBottomClassForLevel();

            return $"responsive-heading handel white shadow{(EffectiveCenterText ? " centered-text" : string.Empty)} {marginBottomClass} {CssClass}".Trim();
        }
    }

    private string ComputedStyle =>
        $"font-size: {ResolveFontSize(FontSize ?? GetDefaultFontSizeForLevel())}; width: {Width}%; {(NoWrap ? "white-space: nowrap; " : string.Empty)}{Style}";

    private static string ResolveFontSize(string fontSize) => fontSize switch
    {
        "xs" => "var(--font-size-xs)",
        "sm" => "var(--font-size-sm)",
        "base" => "var(--font-size-base)",
        "md" => "var(--font-size-md)",
        "lg" => "var(--font-size-lg)",
        "xl" => "var(--font-size-xl)",
        "2xl" => "var(--font-size-2xl)",
        "3xl" => "var(--font-size-2xl)",
        _ => "var(--font-size-2xl)",
    };

    private string GetDefaultFontSizeForLevel() => HeadingLevel switch
    {
        1 => "2xl",
        2 => "2xl",
        3 => "xl",
        4 => "lg",
        5 => "md",
        _ => "base",
    };

    private string GetDefaultMarginBottomClassForLevel() => HeadingLevel switch
    {
        1 => "mb-xl",
        2 => "mb-lg",
        3 => "mb-md",
        4 => "mb-sm",
        5 => "mb-xs",
        _ => "mb-0",
    };
}
