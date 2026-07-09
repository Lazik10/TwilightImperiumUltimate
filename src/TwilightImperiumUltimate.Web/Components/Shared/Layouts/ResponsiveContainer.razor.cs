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
    /// Gets or sets the container type: fluid, centered, narrow, wide.
    /// </summary>
    [Parameter]
    public string Type { get; set; } = "fluid";

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
    /// Gets or sets the click callback.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether child content should be centered using flex layout.
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

    private string Justify => JustifyContent.GetJustifyString();

    private string Align => AlignItems.GetAlignString();

    /// <summary>
    /// Gets the combined CSS class string based on container type and padding.
    /// </summary>
    /// <returns>The CSS class string.</returns>
    private string GetContainerClass()
    {
        var classes = new List<string>();

        // Container type
        classes.Add(Type switch
        {
            "centered" => "container-centered",
            "narrow" => "container-narrow",
            "wide" => "container-wide",
            _ => "container-fluid",
        });

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
        var layoutStyle = CenteredItems
            ? $"display:flex; justify-content:{JustifyContent.Center.GetJustifyString()} align-items:{AlignItems.Center.GetAlignString()}"
            : $"justify-content:{Justify} align-items:{Align}";
        return $"width: {Width}%; {cursorStyle} {layoutStyle} {Style}";
    }
}
