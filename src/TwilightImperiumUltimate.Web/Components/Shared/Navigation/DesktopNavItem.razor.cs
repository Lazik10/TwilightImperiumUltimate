using TwilightImperiumUltimate.Web.Enums;
using TwilightImperiumUltimate.Web.Services.Navigation;

namespace TwilightImperiumUltimate.Web.Components.Shared.Navigation;

/// <summary>
/// Single top-level desktop navigation entry (plain link or hover/focus dropdown trigger).
/// </summary>
public partial class DesktopNavItem
{
    private bool _isHovered;
    private string? _hoveredSubMenuKey;

    /// <summary>
    /// Gets or sets the identifier for this navigation section.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public MainMenuKey MenuKey { get; set; }

    /// <summary>
    /// Gets or sets the localized label for this navigation section.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public string Text { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the link target used when this section has no dropdown (for example "News").
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Gets or sets the dropdown links for this section. When null or empty, a plain link
    /// using <see cref="Href"/> is rendered instead.
    /// </summary>
    [Parameter]
    public IReadOnlyList<NavLinkItem>? Items { get; set; }

    [Inject]
    private IMenuSelectionState MenuSelectionState { get; set; } = default!;

    private string DropdownId => $"desktop-submenu-{MenuKey.ToString().ToLowerInvariant()}";

    private bool IsActiveMenu => MenuSelectionState.ActiveMenuKey == MenuKey;

    private void HandleHoverStart()
    {
        _isHovered = true;
        _hoveredSubMenuKey = null;
    }

    private void HandleHoverEnd()
    {
        _isHovered = false;
        _hoveredSubMenuKey = null;
    }

    private void HandleSubMenuHoverStart(string subMenuKey) => _hoveredSubMenuKey = subMenuKey;

    private void HandleSelectMenu() => MenuSelectionState.SelectMenu(MenuKey);

    private void HandleSelectSubMenu(string subMenuKey) => MenuSelectionState.SelectSubMenu(MenuKey, subMenuKey);

    private string GetActiveCssClass() => _isHovered || IsActiveMenu ? "menu-link-active" : string.Empty;

    private string GetSubMenuCssClass(string subMenuKey)
    {
        if (_hoveredSubMenuKey is not null)
        {
            return string.Equals(_hoveredSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase)
                ? "submenu-link-active"
                : string.Empty;
        }

        return string.Equals(MenuSelectionState.ActiveSubMenuKey, subMenuKey, StringComparison.OrdinalIgnoreCase)
            ? "submenu-link-active"
            : string.Empty;
    }
}
