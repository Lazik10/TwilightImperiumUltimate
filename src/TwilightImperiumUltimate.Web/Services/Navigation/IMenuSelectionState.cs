using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.Navigation;

public interface IMenuSelectionState
{
    /// <summary>
    /// Raised whenever any selection or hover state changes, so unrelated components
    /// (for example sibling desktop nav items) can re-render to reflect the new state.
    /// </summary>
    event Action? Changed;

    MainMenuKey? ActiveMenuKey { get; }

    string? ActiveSubMenuKey { get; }

    /// <summary>
    /// Gets the top-level menu currently being hovered/focused on desktop, if any. Used to
    /// suppress the "active" (current page) highlight on other menus while the user is
    /// interacting with a different menu's dropdown.
    /// </summary>
    MainMenuKey? HoveredMenuKey { get; }

    void SelectMenu(MainMenuKey menuKey);

    void SelectSubMenu(MainMenuKey menuKey, string subMenuKey);

    void ClearSelection();

    void SetHoveredMenu(MainMenuKey menuKey);

    void ClearHoveredMenu(MainMenuKey menuKey);
}
