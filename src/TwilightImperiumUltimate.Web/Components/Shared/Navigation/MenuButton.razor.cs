namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// Reusable, accessible menu button used for menu triggers and actions that do not navigate
/// to a URL (dropdown toggles, logout, mobile menu toggle). Renders a native button element
/// so it is keyboard operable and exposed correctly to assistive technology.
/// </summary>
public partial class MenuButton
{
    /// <summary>
    /// Gets or sets the button content.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Gets or sets additional CSS classes controlling the visual appearance for the current context.
    /// </summary>
    [Parameter]
    public string CssClass { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the click handler invoked when the button is activated.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the button opens a popup (adds aria-haspopup).
    /// </summary>
    [Parameter]
    public bool AriaHasPopup { get; set; }

    /// <summary>
    /// Gets or sets the expanded state of the controlled popup. When null, aria-expanded is omitted.
    /// </summary>
    [Parameter]
    public bool? AriaExpanded { get; set; }

    /// <summary>
    /// Gets or sets the id of the element controlled by this button (aria-controls).
    /// </summary>
    [Parameter]
    public string? AriaControls { get; set; }

    /// <summary>
    /// Gets or sets an accessible name for icon-only or otherwise unlabeled buttons.
    /// </summary>
    [Parameter]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Gets or sets the native title tooltip text.
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    private string? AriaHasPopupValue => AriaHasPopup ? "true" : null;

    private string? AriaExpandedValue => AriaExpanded switch
    {
        true => "true",
        false => "false",
        null => null,
    };

    private Task HandleClickAsync() => OnClick.InvokeAsync();
}
