using TagName = Radzen.Blazor.TagName;
using TextStyle = Radzen.Blazor.TextStyle;

namespace TwilightImperiumUltimate.Web.Components.Shared.Text;

/// <summary>
/// Base component for all text elements with responsive typography.
/// </summary>
public abstract class ResponsiveTextBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the text content to display.
    /// </summary>
    [Parameter]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional CSS classes to apply to the component.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets additional inline styles to apply to the component.
    /// </summary>
    [Parameter]
    public string Style { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the text color using the TextColor enumeration.
    /// </summary>
    [Parameter]
    public TextColor TextColor { get; set; } = TextColor.White;

    /// <summary>
    /// Gets or sets a value indicating whether the text should be centered.
    /// </summary>
    [Parameter]
    public bool CenterText { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether the text is visible.
    /// </summary>
    [Parameter]
    public bool Visible { get; set; } = true;

    /// <summary>
    /// Gets or sets the font size using CSS custom property names (--font-size-*).
    /// Options: xs, sm, base, md, lg, xl, 2xl, 3xl.
    /// </summary>
    [Parameter]
    public string FontSize { get; set; } = "base";

    /// <summary>
    /// Gets or sets the Radzen text style.
    /// </summary>
    [Parameter]
    public TextStyle TextStyle { get; set; } = TextStyle.Body1;

    /// <summary>
    /// Gets or sets the HTML tag used for the text element.
    /// </summary>
    [Parameter]
    public TagName TagName { get; set; } = TagName.P;

    /// <summary>
    /// Gets or sets optional child content for components that support templated text rendering.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets the CSS color class based on the TextColor property.
    /// </summary>
    /// <returns>The CSS color value as a string.</returns>
    protected string GetColorClass()
    {
        return TextColor switch
        {
            TextColor.White => "white",
            TextColor.Red => "red",
            TextColor.Green => "lawngreen",
            TextColor.Blue => "blue",
            TextColor.Yellow => "yellow",
            TextColor.Deepskyblue => "deepskyblue",
            TextColor.Purple => "purple",
            TextColor.Pink => "magenta",
            TextColor.Orange => "orange",
            TextColor.Grey => "grey",
            TextColor.DarkGreen => "darkgreen",
            TextColor.Black => "black",
            _ => "white",
        };
    }

    /// <summary>
    /// Gets the CSS custom property value for the font size.
    /// </summary>
    /// <returns>The CSS variable reference for the specified font size.</returns>
    protected string GetFontSizeStyle()
    {
        return FontSize switch
        {
            "xs" => "var(--font-size-xs)",
            "sm" => "var(--font-size-sm)",
            "base" => "var(--font-size-base)",
            "md" => "var(--font-size-md)",
            "lg" => "var(--font-size-lg)",
            "xl" => "var(--font-size-xl)",
            "2xl" => "var(--font-size-2xl)",
            "3xl" => "var(--font-size-3xl)",
            _ => "var(--font-size-base)",
        };
    }

    /// <summary>
    /// Gets the visibility CSS class based on the Visible property.
    /// </summary>
    /// <returns>An empty string if visible, otherwise 'invisible-text'.</returns>
    protected string GetVisibilityClass() => Visible ? string.Empty : "invisible-text";

    /// <summary>
    /// Gets the text alignment inline style based on the CenterText property.
    /// </summary>
    /// <returns>The text-align CSS property if centered, otherwise empty string.</returns>
    protected string GetTextAlignmentStyle() => CenterText ? "text-align: center;" : string.Empty;
}
