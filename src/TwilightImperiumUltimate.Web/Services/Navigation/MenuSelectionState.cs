namespace TwilightImperiumUltimate.Web.Services.Navigation;

public class MenuSelectionState : IMenuSelectionState
{
    public string? ActiveMenuKey { get; private set; }

    public string? ActiveSubMenuKey { get; private set; }

    public void SelectMenu(string menuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = null;
    }

    public void SelectSubMenu(string menuKey, string subMenuKey)
    {
        ActiveMenuKey = menuKey;
        ActiveSubMenuKey = subMenuKey;
    }
}
