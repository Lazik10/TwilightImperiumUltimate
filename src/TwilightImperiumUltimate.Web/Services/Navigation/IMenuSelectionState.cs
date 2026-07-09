using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.Navigation;

public interface IMenuSelectionState
{
    MainMenuKey? ActiveMenuKey { get; }

    string? ActiveSubMenuKey { get; }

    void SelectMenu(MainMenuKey menuKey);

    void SelectSubMenu(MainMenuKey menuKey, string subMenuKey);
}
