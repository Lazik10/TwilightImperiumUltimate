namespace TwilightImperiumUltimate.Web.Components.Shared.Layouts;

/// <summary>
/// Base class for shared layout container parameters and behavior.
/// </summary>
public abstract class LayoutContainerBase : ComponentBase
{
    /// <summary>
    /// Gets or sets the child content rendered inside the container.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

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
    /// Gets the cursor style based on click handler availability.
    /// </summary>
    /// <returns>The cursor style segment.</returns>
    protected string GetCursorStyle() => OnClick.HasDelegate ? "cursor: pointer;" : string.Empty;
}
