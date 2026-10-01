using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.Navigation;

namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// Single collapsible mobile navigation section (toggle row + expandable link list).
/// </summary>
public partial class MobileNavSection
{
    /// <summary>
    /// Gets or sets the identifier for this navigation section.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public MainMenuKey MenuKey { get; set; }

    /// <summary>
    /// Gets or sets the localized label for this navigation section's toggle row.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the links shown when this section is expanded.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public IReadOnlyList<NavLinkItem> Items { get; set; } = [];

    /// <summary>
    /// Gets or sets a value indicating whether this section's link list is expanded.
    /// </summary>
    [Parameter]
    public bool IsExpanded { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked when the toggle row is activated.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback OnToggle { get; set; }

    /// <summary>
    /// Gets or sets the callback invoked with the selected link's key when a sub-menu link is activated.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public EventCallback<string> OnSelectSubMenu { get; set; }

    [Inject]
    private IMenuSelectionState MenuSelectionState { get; set; } = default!;

    private string SubMenuId => $"mobile-submenu-{MenuKey.ToString().ToLowerInvariant()}";

    private string GetActiveCssClass() =>
        MenuSelectionState.ActiveMenuKey == MenuKey ? "menu-link-active" : string.Empty;

    private string GetSubMenuCssClass(string subMenuKey) =>
        string.Equals(MenuSelectionState.ActiveSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase)
            ? "submenu-link-active"
            : string.Empty;
}
