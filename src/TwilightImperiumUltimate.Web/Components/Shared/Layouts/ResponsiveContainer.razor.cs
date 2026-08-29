using TwilightImperiumUltimate.Web.Helpers.Enums;

namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

/// <summary>
/// Code-behind for <see cref="ResponsiveContainer"/>.
/// </summary>
public partial class ResponsiveContainer
{
    /// <summary>
    /// Gets or sets the child content to render inside the container.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets an optional debug/identification name, rendered as a "data-container-name"
    /// attribute. Not used for styling or behavior -- purely so a specific call site can be found
    /// quickly both in the browser DevTools element tree and by searching the codebase for the
    /// same string. Left null/empty by default, in which case no attribute is rendered at all.
    /// </summary>
    [Parameter]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the container type: Fluid, Centered, Narrow, Wide, or Flex.
    /// </summary>
    [Parameter]
    public ContainerType Type { get; set; } = ContainerType.Fluid;

    /// <summary>
    /// Gets or sets the padding size: none, xs, sm, md, lg, xl.
    /// </summary>
    [Parameter]
    public string Padding { get; set; } = "md";

    /// <summary>
    /// Gets or sets additional CSS classes.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the width in percent.
    /// </summary>
    [Parameter]
    public int Width { get; set; } = 100;

    /// <summary>
    /// Gets or sets the paragraph font-size token/value applied to descendant <c>p</c> tags.
    /// Accepts the shared scale tokens (xs/sm/base/md/lg/xl/2xl/3xl) or any valid CSS
    /// font-size value such as <c>1rem</c> or <c>clamp(...)</c>.
    /// </summary>
    [Parameter]
    public string ParagraphFontSize { get; set; } = "base";

    /// <summary>
    /// Gets or sets the click callback.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether child content should be centered using flex layout.
    /// When true, overrides Type and emits display: flex with justify-content and align-items both set to center.
    /// </summary>
    [Parameter]
    public bool CenteredItems { get; set; }

    /// <summary>
    /// Gets or sets the flex justify-content value.
    /// Ignored when CenteredItems is true.
    /// </summary>
    [Parameter]
    public JustifyContent JustifyContent { get; set; } = JustifyContent.FlexStart;

    /// <summary>
    /// Gets or sets the flex align-items value.
    /// Ignored when CenteredItems is true.
    /// </summary>
    [Parameter]
    public AlignItems AlignItems { get; set; } = AlignItems.FlexStart;

    /// <summary>
    /// Gets or sets the flex direction.
    /// Only used when Type is Flex.
    /// Default is Row.
    /// </summary>
    [Parameter]
    public FlexDirection Direction { get; set; } = FlexDirection.Row;

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();

    /// <summary>
    /// Gets the combined CSS class string based on container type and padding.
    /// </summary>
    /// <returns>The CSS class string.</returns>
    private string GetContainerClass()
    {
        var classes = new List<string>();

        // Container type (Flex type does not add a container-* class; flex layout is applied via inline style)
        if (Type != ContainerType.Flex)
        {
            classes.Add(Type switch
            {
                ContainerType.Centered => "container-centered",
                ContainerType.Narrow => "container-narrow",
                ContainerType.Wide => "container-wide",
                _ => "container-fluid",
            });
        }

        // Padding
        if (Padding != "none")
        {
            classes.Add($"container-padding-{Padding}");
        }

        return string.Join(" ", classes);
    }

    /// <summary>
    /// Gets the composed inline style for width, cursor, and custom styles.
    /// </summary>
    /// <returns>The inline style string.</returns>
    private string GetContainerStyle()
    {
        var cursorStyle = OnClick.HasDelegate ? "cursor: pointer;" : string.Empty;
        var layoutStyle = GetLayoutStyle();
        return $"--container-paragraph-font-size: {ResolveParagraphFontSize()}; width: {Width}%; {cursorStyle} {layoutStyle} {Style}";
    }

    /// <summary>
    /// Gets the layout style based on CenteredItems and Type.
    /// </summary>
    private string GetLayoutStyle()
    {
        if (CenteredItems)
        {
            return $"display:flex; flex-direction:{Direction.GetFlexDirectionString()} justify-content:{JustifyContent.Center.GetJustifyString()} align-items:{AlignItems.Center.GetAlignString()}";
        }

        if (Type == ContainerType.Flex)
        {
            return $"display:flex; flex-direction:{Direction.GetFlexDirectionString()} justify-content:{Justify} align-items:{Align}";
        }

        return $"justify-content:{Justify} align-items:{Align}";
    }

    private string ResolveParagraphFontSize() => ParagraphFontSize switch
    {
        "xs" => "var(--font-size-xs)",
        "sm" => "var(--font-size-sm)",
        "base" => "var(--font-size-base)",
        "md" => "var(--font-size-md)",
        "lg" => "var(--font-size-lg)",
        "xl" => "var(--font-size-xl)",
        "2xl" => "var(--font-size-2xl)",
        "3xl" => "var(--font-size-3xl)",
        null or "" => "var(--font-size-base)",
        _ => ParagraphFontSize,
    };
}
