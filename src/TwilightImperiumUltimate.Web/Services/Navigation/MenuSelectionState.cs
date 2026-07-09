using TwilightImperiumUltimate.Web.Enums;

namespace TwilightImperiumUltimate.Web.Services.Navigation;

public class MenuSelectionState : IMenuSelectionState
{
    public MainMenuKey? ActiveMenuKey { get; private set; }

    public string? ActiveSubMenuKey { get; private set; }

    public void SelectMenu(MainMenuKey menuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = null;
    }

    public void SelectSubMenu(MainMenuKey menuKey, string subMenuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = subMenuKey;
    }
}
